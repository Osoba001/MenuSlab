using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class MenuItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required decimal PackagePrice { get; set; }
        public decimal Discount { get; set; }
        public string DiscountCondiction { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public int Index { get; set; }
        public List<OrderedItem> OrderedItems { get; set; } = [];
        public required Guid CategoryId { get; set; }
        public MenuCategory Category { get; set; }
    }
    internal class MenuItemEntityConfig : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Category).WithMany(x => x.Items).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CategoryId);
        }
    }
}
