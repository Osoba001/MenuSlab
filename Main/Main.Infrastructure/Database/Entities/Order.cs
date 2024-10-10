using Main.Application.CustomTypes;
using Main.Application.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class Order
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public required Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public List<OrderedItem> OrderedItems { get; set; } = [];
        public List<Fund> Payments { get; set; } = [];
        public decimal AmountChargeBaseOn { get; set; }
        public decimal DeliveryPaid=> Payments.Where(x=>x.FundTransactionType==FundTransactionType.PayForDelivery).Sum(x=>x.Amount);
        public decimal OrderPaid=> Payments.Where(x=>x.FundTransactionType==FundTransactionType.PayForOrder).Sum(x=>x.Amount);
        public decimal RestaurantAmount => OrderedItems.Sum(x => x.UnitPrice*x.Quantity);
        public decimal OrderCharges => ComputeorderFee(Restaurant.Country.BaseOrderChange);
        public decimal DeliveryFee { get; set; } 
        public decimal DeliveryCharges => DeliveryFee * 0.1m;
        public required OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }=OrderStatus.PendingByRestaurant;
        public DateTime? WaitTimeInMinutes { get; set; }
        public string? TableNumber { get; set; }
        public Coordinates? DeliveryCoordinates { get; set; }
        public Guid? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public string Completedby { get; set; } = string.Empty;
        public string DeclineMessage { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public Guid? DispatcherId {  get; set; }
        public Dispatcher? Dispatcher { get; set; }
        public Guid? RestaurantStaffId { get; set; }
        public RestaurantStaff? RestaurantStaff { get; set; }
        public DeleteStatus Delete { get; set; } = DeleteStatus.None;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public List<RestaurantRating> RestaurantRatings { get; set; } = [];
        public List<DispatcherRating> DispatcherRatings { get; set; } = [];

        private decimal ComputeorderFee(decimal baseCharge)
        {
            decimal cond = AmountChargeBaseOn * 0.01m/baseCharge;
            decimal charge = baseCharge;
            if (cond < 1)
                charge += 0;
            else if(cond <3)
                charge += charge;
            else if(cond < 6)
                charge += charge*2;
            else if(cond < 10)
                charge += charge*3;
            else
                charge += charge*4;
            if (OrderType != OrderType.SelfOnPremise)
                return charge * 2;
            return charge;
        }

    }
    internal class OrderEntityConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Restaurant).WithMany(x => x.Orders).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany(x => x.SentOrders).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Receiver).WithMany(x => x.ReceivedOrders).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasOne(x => x.Dispatcher).WithMany(x => x.Orders).HasForeignKey(x => x.DispatcherId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasOne(x => x.RestaurantStaff).WithMany(rs => rs.Orders).HasForeignKey(x => x.RestaurantStaffId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.RestaurantId);
            builder.OwnsOne(x => x.DeliveryCoordinates, bd =>
            {
                bd.Property(c => c.Latitude).IsRequired();
                bd.Property(c => c.Longitude).IsRequired();
            });
        }
    }
}
