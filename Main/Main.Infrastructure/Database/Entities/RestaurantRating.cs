using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Main.Infrastructure.Database.Entities
{
    internal class RestaurantRating
    {
        public required Guid OrderId { get; set; }
        public Order Order { get; set; }
        public required int Value { get; set; }
        public required string Message { get; set; }
        public required Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
   

    internal class RestaurantRatingEntityConfig : IEntityTypeConfiguration<RestaurantRating>
    {
        public void Configure(EntityTypeBuilder<RestaurantRating> builder)
        {
            builder.HasNoKey();
            builder.HasOne(x=>x.Order).WithMany(x=>x.RestaurantRatings).HasForeignKey(x=>x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.RestaurantId);
            builder.HasIndex(x=>x.OrderId).IsUnique();
        }
    }

}
