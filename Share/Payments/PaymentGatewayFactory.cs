namespace Share.Payments
{
    public class PaymentGatewayFactory(IEnumerable<IPaymentGateway> paymentGateways) : IPaymentGatewayFactory
    {
        private readonly IEnumerable<IPaymentGateway> _paymentGateways = paymentGateways;

        public IPaymentGateway GetPaymentGateway(string gatewayName)
        {
            var paymentGateway = _paymentGateways.FirstOrDefault(pg =>
                pg.GetType().Name.Contains(gatewayName, StringComparison.CurrentCultureIgnoreCase));

            return paymentGateway ?? throw new NotSupportedException($"Payment gateway '{gatewayName}' is not supported.");
        }
    }
}
