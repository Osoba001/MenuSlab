using Main.Application.CustomTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Main.Infrastructure.Database.Entities.EntityBase;

namespace Main.Infrastructure.Database.Entities
{
    internal class Dispatcher: OrderProviderBase
    {
        public required string Rider { get; set; }
        public List<DispatcherRating> Ratings { get; set; } = [];
    }
    internal class DispatcherEntityConfig : IEntityTypeConfiguration<Dispatcher>
    {
        public void Configure(EntityTypeBuilder<Dispatcher> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.BankDetails, bd =>
            {
                bd.Property(b => b.Bank).IsRequired();
                bd.Property(b => b.AccountNo).IsRequired();
                bd.Property(b => b.AccountName).IsRequired();
            });
            builder.HasIndex(x => x.UserId);

            builder.HasOne(x => x.User)
              .WithMany(x => x.Dispatchers)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Country)
              .WithMany(x => x.Dispatchers)
              .HasForeignKey(x => x.CountryId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
