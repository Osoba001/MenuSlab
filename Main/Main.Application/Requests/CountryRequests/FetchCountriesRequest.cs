namespace Main.Application.Requests.CountryRequests
{
    public class FetchCountriesRequest : Request
    {
        
    }
    public class CountryResponse
    {
        public required string Name { get; set; }
        public required string Currency { get; set; }
    }
    
}
