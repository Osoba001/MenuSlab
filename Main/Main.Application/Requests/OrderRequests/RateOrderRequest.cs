namespace Main.Application.Requests.OrderRequests
{
    public class RateOrderRequest : Request
    {
        public required Guid OrderId { get; set; }
        public string Message { get; set; } = string.Empty;
        public required int Value { get; set; }

        public override ActionResponse Validate()
        {
            if (Value > 5 || Value < 1)
                return BadRequestResult("Rating value should be between 1 to 5");
            return base.Validate();
        }
    }

}
