using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace AstrolPOSAPI.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMediator();
            services.AddValidators();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });
        }


        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static void AddValidators(this IServiceCollection services)
        {
            // Manual registration - scan and register all validators
            var validatorTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                           t.GetInterfaces().Any(i =>
                               i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IValidator<>)));

            foreach (var validatorType in validatorTypes)
            {
                var validatorInterface = validatorType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                services.AddScoped(validatorInterface, validatorType);
            }
        }
    }
}
