using Main.Application.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class Fund
    {
        public Guid Id { get; set; }
        public required decimal Amount { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public required FundTransactionType FundTransactionType { get; set; }
        public Guid? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public Guid? SenderId { get; set; }
        public User? Sender { get; set; }
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
    }
    internal class FundEntityConfig : IEntityTypeConfiguration<Fund>
    {
        public void Configure(EntityTypeBuilder<Fund> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Receiver).WithMany(x => x.ReceivedFunds).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasOne(x => x.Sender).WithMany(x => x.SentFunds).HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasOne(x => x.Order).WithMany(x => x.Payments).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasIndex(x => x.SenderId);
            builder.HasIndex(x => x.ReceiverId);
        }
    }
}
