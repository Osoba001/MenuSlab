using Microsoft.Extensions.DependencyInjection;
using Share.EmailService;
using Share.FileManagement;
using Share.MediatKO;
using Share.Payments;
using Swinva.share;
using System.Reflection;

namespace Share
{
    public static class ShareServicesRegistration
    {

        public static IServiceCollection AddShareServices(this IServiceCollection services, Action<ShareConfigData> options)
        {
            var config = new ShareConfigData();
            options?.Invoke(config);

            services.AddSingleton(config.EMAIL_CONFIGURATION);
            services.AddSingleton(config.DEPLOYMENT_CONFIGURATION);
            services.AddSingleton(config.ObjectStorageConfiguration);
            services.AddScoped<IFileManagementService, ObjectStorageService>();
            services.AddScoped<IMailSender, EmailKitService>();
            services.AddScoped<IMediator, Mediator>();

            services.AddPaymentGatewayService();
            return services;
        }

        private static IServiceCollection AddPaymentGatewayService(this IServiceCollection services)
        {
            //
            var paymentTypes = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(t => typeof(IPaymentGateway).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var paymentType in paymentTypes)
            {
                services.AddScoped(typeof(IPaymentGateway), paymentType);
            }
            services.AddSingleton<IPaymentGatewayFactory,PaymentGatewayFactory>();

            return services;
        }
    }
}
