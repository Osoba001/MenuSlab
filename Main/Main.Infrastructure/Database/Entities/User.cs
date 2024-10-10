using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Scrypt;
using Main.Application.Requests.UserRequests;

namespace Main.Infrastructure.Database.Entities
{
    [Table("users")]
    internal class User
    {
        public Guid Id { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public required string AuthenticationType { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string HashPin { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime AllowSetNewPassword { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public int PasswordRecoveryPin { get; set; }
        public DateTime RecoveryPinCreationTime { get; set; }
        public byte Attempt { get; set; } = 0;
        public DateTime WhenToUnlockInMinutes { get; set; }
        public bool IsLock { get; set; }
        public string? LockMessage { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public string Session { get; set; }=string.Empty;
        public required string UserDevice { get; set; }
        public decimal Balance { get; set; }
        public string PaymentPinHash { get; set; } = string.Empty;
        public required string CountryId { get; set; }
        public Country Country { get; set; }
        public List<Dispatcher> Dispatchers { get; set; } = [];
        public List<Restaurant> Restaurants { get; set; } = [];
        public List<Order> SentOrders { get; set; } = [];
        public List<Order> ReceivedOrders {  get; set; } = [];
        public List<Permission> Permissions { get; set; } = [];
        public List<Fund> SentFunds { get; set; } = [];
        public List<Fund> ReceivedFunds { get; set; } = [];
        public List<OfferOrder> OfferOrderSenders { get; set; } = [];
        public List<OfferOrder> ReceivedOfferOrder { get; set; } = [];
        public RestaurantStaff? RestaurantStaff { get; set; }
        public SystemAdmin? SystemAdmin { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public static string HashPassword(string password) => new ScryptEncoder().Encode(password);


        public static bool VerifyPassword(string password, string hashedPassword)
            => new ScryptEncoder().Compare(password, hashedPassword);


        public static implicit operator UserResponse(User model)
        {
            return new UserResponse() { Email = model.Email, Id = model.Id, Role = model.Role.ToString(), CountryId = model.CountryId, Name = model.Name, PhoneNo = model.PhoneNo };
        }
    }

    internal class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(225);
            builder.Property(x => x.Role).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasOne(x => x.RestaurantStaff).WithOne(x => x.User).HasForeignKey<RestaurantStaff>(x => x.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            builder.HasOne(x => x.SystemAdmin).WithOne(x => x.User).HasForeignKey<SystemAdmin>(x => x.UserId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasOne(x => x.Country)
              .WithMany(x => x.Users)
              .HasForeignKey(x => x.CountryId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
