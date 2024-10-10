namespace Main.Application.Requests.MenuRequests
{
    public class FetchMenuRequest : Request
    {
        public required Guid RestaurantId { get; set; }
    }

    public class FetchMenuResponse
    {
        public required Guid Id { get; set; }
        public required string CategoryName { get; set; }
        public required int Index { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public required bool IsRoot { get; set; }
        public required List<MenuItemResponse> Items { get; set; }
    }
    public class MenuItemResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required decimal PackagePrice { get; set; }
        public decimal Discount { get; set; }
        public required string DiscountCondiction { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int Index { get; set; }
    }
}
