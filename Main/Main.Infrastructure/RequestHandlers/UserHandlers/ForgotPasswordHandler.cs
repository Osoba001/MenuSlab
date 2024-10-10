using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Mails.PasswordResetMail;
using Microsoft.EntityFrameworkCore;
using Share.EmailService;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class ForgotPasswordHandler(IMailGenerator<PasswordResetPayload> mailGenerator, MenuslabDbContext dbContext) : IRequestHandler<ForgotPasswordRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        private readonly IMailGenerator<PasswordResetPayload> _mailGenerator = mailGenerator;

        public async Task<ActionResponse> HandleAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(x => x.Email == request.Email.ToLower().Trim()).FirstOrDefaultAsync();


            if (user is null)
                return NotFoundResult(UserNotFound);

            int pin = RandomPin();

            user.PasswordRecoveryPin = pin;
            user.RecoveryPinCreationTime = DateTime.UtcNow;
            _dbContext.Users.Entry(user).Property(x => x.PasswordRecoveryPin).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RecoveryPinCreationTime).IsModified = true;

            user.Attempt++;
            _dbContext.Users.Entry(user).Property(x => x.Attempt).IsModified = true;

            _ = await _dbContext.CompleteAsync();
            _ = _mailGenerator.SendAsync(new PasswordResetPayload { Receiver = request.Email, RecoveryPin = pin });
            //return new ActionResponse { Data=new { pin } };
            return SuccessResult();
        }

        private static int RandomPin()
        {
            var random = new Random();
            return random.Next(100000, 999999);
        }

    }
}
