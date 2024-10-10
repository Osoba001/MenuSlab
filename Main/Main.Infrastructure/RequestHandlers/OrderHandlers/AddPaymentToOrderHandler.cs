using Main.Application.Enums;
using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class AddPaymentToOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<AddPaymentToOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddPaymentToOrderRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(request.UserIdentifier);
            if (user == null) return NotFoundResult();


            decimal amount = request.AmountForOrder + request.AmountForDelivery;
            switch (request.OrderPaymentType)
            {
                case OrderPaymentType.OrderOnly:
                    amount = request.AmountForOrder;
                    break;
                case OrderPaymentType.DeliveryOnly:
                    amount = request.AmountForDelivery;
                    break;
            }

            if (user.Balance < amount)
                return BadRequestResult("Insufficient funds.");

            if (user.UserDevice != request.UserDevice.ToString())
            {
                // send mail
                var subject = "Transaction Attempt Alert - New Device Detected";

                var messageBody = $@"
                                    Dear {user.Name},

                                    A transaction attempt was detected from a new device. 

                                    - Recent login device: {user.UserDevice.ToString()}
                                    - Device used for this attempt: {request.UserDevice.ToString()}

                                    If this was not you, we strongly recommend that you change your password immediately to secure your account.

                                    Please log in and update your password as soon as possible.

                                    Best regards,
                                    Your Security Team
                                ";


                return Forbiden($"Transaction attempt detected from an unrecognized device. Your most recent login was from {user.UserDevice}. Please log in again to authorize the use of your current device ({request.UserDevice.Model}).");

            }

            var order = await _dbContext.Orders.Where(x=>x.Id==request.OrderId)
                .Include(x=>x.Restaurant).FirstOrDefaultAsync();
            if (order == null) return NotFoundResult();

            if (order.Restaurant.CountryId != user.CountryId)
                return Forbiden($"Failed transaction: The order country [{order.Restaurant.CountryId}] and your country [{user.CountryId}] do not match.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                switch (request.OrderPaymentType)
                {
                    case OrderPaymentType.OrderOnly:
                        await ProcessOrderPaymentAsync(user, order, request.AmountForOrder, cancellationToken);
                        break;
                    case OrderPaymentType.DeliveryOnly:
                        await ProcessDeliveryPaymentAsync(user, order, request.AmountForDelivery, cancellationToken);
                        break;
                    default:
                        await ProcessFullPaymentAsync(user, order, request.AmountForOrder, request.AmountForDelivery, cancellationToken);
                        break;
                }

                await transaction.CommitAsync(cancellationToken);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ServerExceptionError(ex);
            }
        }

        private async Task ProcessOrderPaymentAsync(User user, Order order, decimal amount, CancellationToken cancellationToken)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(order.RestaurantId) ?? throw new Exception("Restaurant not found.");
            AdjustBalances(user, restaurant, amount, isRestaurant: true);

            await LogTransactionAsync(user.Id, amount, order.Id, FundTransactionType.PayForOrder, cancellationToken);
        }

        private async Task ProcessDeliveryPaymentAsync(User user, Order order, decimal amount, CancellationToken cancellationToken)
        {
            var dispatcher = await _dbContext.Dispatchers.FindAsync(order.DispatcherId) ?? throw new Exception("Dispatcher not found.");
            AdjustBalances(user, dispatcher, amount, isRestaurant: false);

            await LogTransactionAsync(user.Id, amount, order.Id, FundTransactionType.PayForDelivery, cancellationToken);
        }

        private async Task ProcessFullPaymentAsync(User user, Order order, decimal orderAmount, decimal deliveryAmount, CancellationToken cancellationToken)
        {
            var dispatcher = await _dbContext.Dispatchers.FindAsync(order.DispatcherId);
            var restaurant = await _dbContext.Restaurants.FindAsync(order.RestaurantId);

            if (dispatcher == null) throw new Exception("Dispatcher not found.");
            if (restaurant == null) throw new Exception("Restaurant not found.");

            AdjustBalances(user, restaurant, orderAmount, isRestaurant: true);
            AdjustBalances(user, dispatcher, deliveryAmount, isRestaurant: false);

            await LogTransactionAsync(user.Id, orderAmount, order.Id, FundTransactionType.PayForOrder, cancellationToken);
            await LogTransactionAsync(user.Id, deliveryAmount, order.Id, FundTransactionType.PayForDelivery, cancellationToken);
        }

        private void AdjustBalances(User user, dynamic recipient, decimal amount, bool isRestaurant)
        {
            user.Balance -= amount;
            _dbContext.Entry(user).Property(x => x.Balance).IsModified = true;

            if (isRestaurant)
            {
                var restaurant = (Restaurant)recipient;
                restaurant.Balance += amount;
                _dbContext.Entry(restaurant).Property(x => x.Balance).IsModified = true;
            }
            else
            {
                var dispatcher = (Dispatcher)recipient;
                dispatcher.Balance += amount;
                _dbContext.Entry(dispatcher).Property(x => x.Balance).IsModified = true;
            }
        }

        private async Task LogTransactionAsync(Guid senderId, decimal amount, Guid orderId, FundTransactionType transactionType, CancellationToken cancellationToken)
        {
            var fund = new Fund
            {
                Amount = amount,
                SenderId = senderId,
                FundTransactionType = transactionType,
                OrderId = orderId
            };

            _dbContext.Funds.Add(fund);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
