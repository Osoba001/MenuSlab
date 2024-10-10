namespace Main.Application.Requests.MenuRequests
{
    public class DeleteMenuCategoriesRequest : Request
    {
        public required Guid RestaurantId { get; set; }
        public required List<Guid> DeleteCategoryIds { get; set; }
        public required List<MenuCategoryDto> UpdatedMenuCategories { get; set; }
    }
    public class MenuCategoryDto
    {
        public required Guid Id { get; set; }
        public required int Index { get; set; }
    }

    
}
