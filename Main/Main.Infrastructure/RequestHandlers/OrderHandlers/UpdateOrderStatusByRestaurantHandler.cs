using Main.Application.Enums;
using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class UpdateOrderStatusByRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateOrderStatusByRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateOrderStatusByRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FindAsync(request.Id);
            if (order == null) return NotFoundResult();
            order.Status = request.Status;
            order.DeclineMessage = request.DeclineMessage;
            if (order.RestaurantStaffId == null)
            {
                order.RestaurantStaffId = request.RestaurantStaffId;
                _dbContext.Entry(order).Property(x => x.RestaurantStaffId).IsModified = true;
            }
            if (order.WaitTimeInMinutes == null && request.WaitTimeInMinutes!=null)
            {
                order.WaitTimeInMinutes = DateTime.UtcNow.AddMinutes(request.WaitTimeInMinutes.Value);
                _dbContext.Entry(order).Property(x => x.WaitTimeInMinutes).IsModified = true;
            }
            if (order.OrderType != OrderType.SelfOnPremise && string.IsNullOrEmpty(order.OrderNumber))
            {
                var orderCodes = await _dbContext.Orders.Where(x => x.RestaurantId == order.RestaurantId && x.CreatedDate >= DateTime.UtcNow.AddHours(24) && x.OrderType != OrderType.SelfOnPremise)
                    .Select(x => x.OrderNumber).ToListAsync();
                order.OrderNumber=GenerateRandomCode(orderCodes);
                _dbContext.Entry(order).Property(x=>x.OrderNumber).IsModified = true;
            }
            _dbContext.Entry(order).Property(x => x.Status).IsModified = true;
            _dbContext.Entry(order).Property(x => x.DeclineMessage).IsModified = true;

            return await _dbContext.CompleteAsync(new {order.OrderNumber});
        }

        private static readonly Random _random = new();

        public static string GenerateRandomCode(List<string> activeCodes, int length = 5)
        {
            const string chars = "0123456789";
            string newCode;

            do
            {
                newCode = new string(Enumerable.Repeat(chars, length)
                                               .Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (activeCodes.Contains(newCode));

            return newCode;
        }
    }
}
