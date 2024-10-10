namespace Share.Payments
{
    public interface IPaymentGatewayFactory
    {
        IPaymentGateway GetPaymentGateway(string gatewayName);
    }
}