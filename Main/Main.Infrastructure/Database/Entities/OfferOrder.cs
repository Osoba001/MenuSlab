using Main.Application.CustomTypes;
using Main.Application.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class OfferOrder
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public OfferOderStatus OfferOderStatus { get; set; }=OfferOderStatus.Pending;
        public DeleteStatus Delete { get; set; } = DeleteStatus.None;
        public string Code { get; set; }=string.Empty;
        public Guid? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public Coordinates? DeliveryCoordinates { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
    internal class OfferOrderEntityConfig : IEntityTypeConfiguration<OfferOrder>
    {
        public void Configure(EntityTypeBuilder<OfferOrder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.OfferOrderSenders).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Receiver).WithMany(x => x.ReceivedOfferOrder).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasIndex(x => x.UserId);
            builder.OwnsOne(x => x.DeliveryCoordinates, bd =>
            {
                bd.Property(c => c.Latitude).IsRequired();
                bd.Property(c => c.Longitude).IsRequired();
            });
        }
    }
}
