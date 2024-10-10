using Main.Application.Requests.OfferOrderRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OfferOrderHandlers
{
    internal class CreateOfferOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<CreateOfferOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateOfferOrderRequest request, CancellationToken cancellationToken = default)
        {

            
            var offer= new OfferOrder {UserId=request.UserIdentifier, ReceiverId=request.ReceiverId };
            string code = string.Empty;
            if (request.ReceiverId == null)
            {

                var activeCodes = await _dbContext.OfferOrders.Where(x => x.CreatedDate > DateTime.UtcNow.AddHours(-24)
                && x.OfferOderStatus == Application.Enums.OfferOderStatus.Pending)
               .Select(x => x.Code).ToListAsync();

                offer.OfferOderStatus = Application.Enums.OfferOderStatus.ReceiverNotInApp;
                code = GenerateRandomCode(activeCodes);
                offer.Code = code;

            }
            //send whatsapp msg or sms
            _dbContext.OfferOrders.Add(offer);
            return await _dbContext.CompleteAsync(new { offer.Id, code});
        }

        private static readonly Random _random = new();

        public static string GenerateRandomCode(List<string> activeCodes, int length = 5)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
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
