namespace Main.Application.Requests.DispatcherRequests
{
    public class VerifyDispatcherRequest : Request
    {
        public required List<Guid> Ids { get; set; }
        public required string Number { get; set; }
    }
    
   
}
