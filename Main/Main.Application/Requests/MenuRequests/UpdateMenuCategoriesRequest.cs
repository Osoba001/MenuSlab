namespace Main.Application.Requests.MenuRequests
{
    public class UpdateMenuCategoriesRequest:Request
    {
        public required Guid RestaurantId { get; set; }
        public required List<UpdatetMenuCategoryDto> MenuCategories { get; set; }
    }

    public class UpdatetMenuCategoryDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required int Index { get; set; }
    }

    

}
