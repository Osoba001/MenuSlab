using Main.Application.CustomTypes;
using Main.Infrastructure.Database.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;

namespace Main.Infrastructure.Database.Entities
{
    internal class Restaurant: OrderProviderBase
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Latitude { get; set; }
        public required decimal Longitude { get; set; }
        public required string StreetAddress { get; set; }
        public double RestaurantRadius { get; set; }
        public required bool HomeDelivery { get; set; }
        public string DeliveryCondiction { get; set; } = string.Empty;
        public required bool OnPremise { get; set; }
        public bool PayWithApp { get; set; }
        public string LockedMessage { get; set; }=string.Empty;
        public string OpenTime { get; set; }=string.Empty ;
        public string CloseTime { get; set; }= string.Empty ;
        public string OpenCode { get; set; } = string.Empty;
        public List<License> Licenses { get; set; } = [];
        public List<MenuCategory> Categories { get; set; } = [];
        public List<RestaurantStaff> Staff { get; set; } = [];
        public List<RestaurantRating> Ratings { get; set; } = [];
        
    }
    internal class RestaurantEntityConfig : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Restaurants)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(x => x.BankDetails, bd =>
            {
                bd.Property(b => b.Bank).IsRequired();
                bd.Property(b => b.AccountNo).IsRequired();
                bd.Property(b => b.AccountName).IsRequired();
            });

            builder.HasOne(x => x.Country)
              .WithMany(x => x.Restaurants)
              .HasForeignKey(x => x.CountryId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasIndex(x => x.UserId);
        }
    }

    
}
