namespace Main.Application.Requests.DispatcherRequests
{
    public class UpdateDispatcherRequest : Request
    {
        public required Guid Id { get; set; }
        public required string Rider { get; set; }
        public required string PhoneNo { get; set; }
    }
    
   
}
