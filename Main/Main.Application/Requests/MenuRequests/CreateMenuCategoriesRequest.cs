namespace Main.Application.Requests.MenuRequests
{
    public class CreateMenuCategoriesRequest : Request
    {
        public Guid? ParentId { get; set; }
        public required bool IsRoot { get; set; }
        public required Guid RestaurantId { get; set; }
        public required List<CreateMenuCategoryDto> MenuCategories { get; set; }
    }


    public class CreateMenuCategoryDto
    {
        public required string Name { get; set; }
        public required int Index { get; set; }
    }
}
