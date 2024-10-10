namespace Main.Application.Requests.MenuRequests
{
    public class DeleteMenuItemsRequest : Request
    {
        public required Guid RestaurantId { get; set; }
        public required List<Guid> DeleteItemIds { get; set; }
        public required List<MenuItemDto> UpdatedMenuItems { get; set; }
    }
    public class MenuItemDto
    {
        public required Guid Id { get; set; }
        public required int Index { get; set; }
    }

}
