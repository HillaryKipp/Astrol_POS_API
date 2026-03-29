using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Infrastructure.Services;
using AstrolPOSAPI.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AstrolPOSAPI.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddServices(configuration);
        }

        private static void AddServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            //services
            //    .AddTransient<IEmailService, EmailService>();

            // NoSeries service for auto-generating codes
            services.AddTransient<AstrolPOSAPI.Application.Interfaces.Services.INoSeriesService,
                Services.NoSeriesService>();

            // Current User Service (for Auditing)
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddTransient<AstrolPOSAPI.Application.Interfaces.Infrastructure.ISmsSender,
                Services.ExpressSmsSender>();

            var httpClientBuilder = services.AddHttpClient("ExpressSMS", client =>
            {
                var baseUrl = configuration["ExpressSMS:BaseUrl"] ?? "https://api.expresssms.com/";
                client.BaseAddress = new Uri(baseUrl);
            });

            // Bypass SSL validation if configured (useful for APIs with certificate issues)
            var bypassSsl = configuration.GetValue<bool>("ExpressSMS:BypassSSL", true); // Default true for compatibility
            if (bypassSsl)
            {
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                });
            }

            // M-Pesa Service
            services.Configure<MpesaSettings>(configuration.GetSection(MpesaSettings.SectionName));
            services.AddTransient<IMpesaService, MpesaService>();

            var mpesaHttpClientBuilder = services.AddHttpClient<IMpesaService, MpesaService>((serviceProvider, client) =>
            {
                var settings = configuration.GetSection(MpesaSettings.SectionName).Get<MpesaSettings>();
                client.BaseAddress = new Uri(settings?.BaseUrl ?? "https://sandbox.safaricom.co.ke");
            });

            // Bypass SSL for M-Pesa if in sandbox
            if (configuration.GetValue<string>("Mpesa:Environment") == "sandbox")
            {
                mpesaHttpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                });
            }
        }
    }
}
