using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Main.Infrastructure.Database.Entities
{
    internal class Bank
    {
        public Guid Id { get; set; }
        public required string BankName { get; set; }
        public required string BankCode { get; set; }
        public required string CountryId { get; set; }
        public Country Country { get; set; }
    }
    internal class BankEntityConfig : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x=>x.Country).WithMany(x=>x.Banks).HasForeignKey(x=>x.CountryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
