using Main.Application.CustomTypes;

namespace Main.Infrastructure.Database.Entities.EntityBase
{
    internal abstract class OrderProviderBase
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public required string CountryId { get; set; }
        public Country Country { get; set; }
        public required string PhoneNo { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime? VerifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string ActiveMessage { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal Charges { get; set; }
        public decimal NetBalance => (Balance - Charges);
        public List<Order> Orders { get; set; } = [];
        public List<Withdrawal> Withdrawals { get; set; } = [];
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public BankAccountDetails? BankDetails { get; set; }
        public bool BanckDetailVerified { get; set; }
        public int WithrewWithThisAccount { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; } = null;
        public bool IsLocked { get; set; } = false;
    }
}
