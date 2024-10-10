using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class DispatcherRating
    {
        public required Guid OrderId { get; set; }
        public Order Order { get; set; }
        public required int Value { get; set; }
        public required Guid DispatcherId { get; set; }
        public Dispatcher Dispatcher { get; set; }
    }
    internal class DispatcherRatingEntityConfig : IEntityTypeConfiguration<DispatcherRating>
    {
        public void Configure(EntityTypeBuilder<DispatcherRating> builder)
        {
            builder.HasNoKey();
            builder.HasOne(x => x.Order).WithMany(x => x.DispatcherRatings).HasForeignKey(x => x.DispatcherId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.DispatcherId);
            builder.HasIndex(x => x.OrderId).IsUnique();
        }
    }
}
