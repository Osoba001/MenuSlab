using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class OrderedItem
    {
        public required Guid OrderId { get; set; }
        public Order Order { get; set; }
        public required Guid ItemId { get; set; }
        public MenuItem Item { get; set; }
        public decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }

    }
    internal class OrderedItemEntityConfig : IEntityTypeConfiguration<OrderedItem>
    {
        public void Configure(EntityTypeBuilder<OrderedItem> builder)
        {
            builder.HasOne(x => x.Order).WithMany(x => x.OrderedItems).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Item).WithMany(x => x.OrderedItems).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(x => x.OrderId);
        }
    }
}
