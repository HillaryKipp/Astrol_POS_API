using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AstrolPOSAPI.Persistence.Contexts;
using AstrolPOSAPI.Application.Interfaces.Services;
using System.Linq;

namespace AstrolPOSAPI.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove existing AppDbContext registration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // Add InMemory AppDbContext
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Mock ICurrentUserService
                services.Remove(services.Single(d => d.ServiceType == typeof(ICurrentUserService)));
                services.AddTransient<ICurrentUserService>(provider => new TestCurrentUserService());
            });
        }

        private class TestCurrentUserService : ICurrentUserService
        {
            public string? UserId => "test-user-id";
        }
    }
}
