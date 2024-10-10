using Microsoft.AspNetCore.Http;

namespace Main.Application.Requests.MenuRequests
{
    public class CreateMenuItemRequest : Request
    {
        public required Guid CategoryId { get; set; }
        public required List<CreateMenuItemDto> Items { get; set; }
    }
    public class CreateMenuItemDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required decimal PackagePrice { get; set; }
        public decimal Discount { get; set; }
        public required string DiscountCondiction { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int Index { get; set; }
        public required IFormFile Photo { get; set; }
    }
}
