using Main.Application.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Main.Application.CustomTypes;

namespace Main.Infrastructure.Database.Entities
{
    internal class Withdrawal
    {
        public Guid Id { get; set; }
        public required decimal Amount { get; set; }
        public required decimal NetBalance { get; set; }
        public WithdrawStatus WithdrawStatus { get; set; } = WithdrawStatus.Pending;
        public string Message { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedDate { get; set; }
        public required string Number { get; set; }
        public required WithdrawalSourceType SourceType { get; set; }
        public required string CountryId { get; set; }
        public Country Country { get; set; }
        public Guid? SystemAdminId { get; set; }
        public SystemAdmin? SystemAdmin { get; set; }

        public Guid? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public Guid? DispatcherId { get; set; }
        public Dispatcher? Dispatcher { get; set; }
        public required BankAccountDetails BankDetails { get; set; }
    }

    internal class WithdrawalEntityConfig : IEntityTypeConfiguration<Withdrawal>
    {
        public void Configure(EntityTypeBuilder<Withdrawal> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne(x => x.Restaurant)
                   .WithMany(x => x.Withdrawals)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasOne(x => x.Dispatcher)
                   .WithMany(x => x.Withdrawals)
                   .HasForeignKey(x => x.DispatcherId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasOne(x => x.SystemAdmin)
                   .WithMany(x => x.Withdrawals)
                   .HasForeignKey(x => x.SystemAdminId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasOne(x => x.Country)
                  .WithMany(x => x.Withdrawals)
                  .HasForeignKey(x => x.CountryId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.OwnsOne(x => x.BankDetails, bd =>
            {
                bd.Property(b => b.Bank).IsRequired();
                bd.Property(b => b.AccountNo).IsRequired();
                bd.Property(b => b.AccountName).IsRequired();
            });
        }
    }
}
