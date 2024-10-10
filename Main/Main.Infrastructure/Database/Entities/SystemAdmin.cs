using Main.Application.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class SystemAdmin
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public required string StaffNumber { get; set; }
        public List<Withdrawal> Withdrawals { get; set; } = [];
        public required string CountryId { get; set; }
        public Country Country { get; set; }
        public StaffRole Role { get; set; }=StaffRole.Staff;
        public bool IsActive { get; set; } = true;
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        //Staff info
    }
    internal class SystemAdminEntityConfig : IEntityTypeConfiguration<SystemAdmin>
    {
        public void Configure(EntityTypeBuilder<SystemAdmin> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Country).WithMany(x => x.SystemAdmins).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
