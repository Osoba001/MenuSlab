namespace Main.Application.Requests.CountryRequests
{
    public class AddCountriesRequest:Request
    {
        public required List<CountryDto> Countries { get; set; }
    }
    public class CountryDto
    {
        public required string Name { get; set; }
        public required string Currency { get; set; }
    }
    
}
