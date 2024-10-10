using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class MenuCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Index { get; set; }

        public required Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public Guid? ParentCategoryId { get; set; }
        public MenuCategory? ParentCategory { get; set; }

        public List<MenuCategory> SubCategories { get; set; } = [];

        public List<MenuItem> Items { get; set; } = [];
    }
    internal class MenuCategoryEntityConfig : IEntityTypeConfiguration<MenuCategory>
    {
        public void Configure(EntityTypeBuilder<MenuCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Restaurant)
                   .WithMany(x => x.Categories)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ParentCategory)
                   .WithMany(x => x.SubCategories)
                   .HasForeignKey(x => x.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.RestaurantId);
        }
    }
}
