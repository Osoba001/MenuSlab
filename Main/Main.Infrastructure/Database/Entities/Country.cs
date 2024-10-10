using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.Infrastructure.Database.Entities
{
    internal class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public decimal MinWithDrawal { get; set; }
        public decimal MaxWithDrawal { get; set; }
        public decimal MaxTransfer { get; set; }
        public decimal MinTransfer { get; set; }
        public decimal BaseOrderChange { get; set; }
        public Guid? PaymentGatewayId { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public List<User> Users { get; set; } = [];
        public List<Restaurant> Restaurants { get; set; } = [];
        public List<Dispatcher> Dispatchers { get; set; } = [];
        public List<SystemAdmin> SystemAdmins { get; set; } = [];
        public List<Withdrawal> Withdrawals { get; set; } = [];
        public List<Bank> Banks { get; set; } = [];
    }
    internal class CountryEntityConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.Name);
            builder.HasOne(e=>e.PaymentGateway).WithMany(x=>x.Countries)
                .HasForeignKey(e=>e.PaymentGatewayId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}
