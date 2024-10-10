using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class RestaurantStaff
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public required string Role { get; set; }
        public string StaffId { get; set; }=string.Empty;
        public required string StartOfDuty { get; set; }
        public required string EndOfDuty { get; set; }
        public bool IsActive { get; set; }=true;
        public required Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public List<Order> Orders { get; set; } = [];
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
    internal class RestaurantStaffEntityConfig : IEntityTypeConfiguration<RestaurantStaff>
    {
        public void Configure(EntityTypeBuilder<RestaurantStaff> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Restaurant).WithMany(x => x.Staff).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
