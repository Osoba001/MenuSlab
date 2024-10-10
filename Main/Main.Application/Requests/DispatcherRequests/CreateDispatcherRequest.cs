namespace Main.Application.Requests.DispatcherRequests
{
    public class CreateDispatcherRequest : Request
    {
        public required string Rider { get; set; }
        public required string PhoneNo { get; set; }
        public required string Country { get; set; }
    }
    
   
}
