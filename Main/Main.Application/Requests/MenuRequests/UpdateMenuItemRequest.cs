using Microsoft.AspNetCore.Http;

namespace Main.Application.Requests.MenuRequests
{
    public class UpdateMenuItemRequest : Request
    {
        public required Guid RestaurantId { get; set; }
        public required List<UpdateMenuItemDto> MenuItems { get; set; }
    }

    public class UpdateMenuItemDto
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
        public IFormFile? Photo { get; set; }=null;
    }

    
}
