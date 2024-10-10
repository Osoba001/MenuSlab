namespace Main.Infrastructure.Database.Entities
{
    internal class FeatureOfRestaurant
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<RestaurantFeature> Restaurants { get; set; } = [];
    }
}
