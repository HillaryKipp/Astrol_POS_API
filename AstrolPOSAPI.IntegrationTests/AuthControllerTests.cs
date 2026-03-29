using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AstrolPOSAPI.IntegrationTests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Register_Then_Login_Should_Return_Jwt()
        {
            var client = _factory.CreateClient();

            var reg = await client.PostAsJsonAsync("/api/auth/register", new
            {
                userName = "testuser",
                password = "P@ssw0rd!",
                name = "Test User"
            });
            reg.EnsureSuccessStatusCode();

            var login = await client.PostAsJsonAsync("/api/auth/login", new
            {
                userName = "testuser",
                password = "P@ssw0rd!"
            });
            login.EnsureSuccessStatusCode();

            var body = await login.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            body!.Should().ContainKey("token");
            body!["token"].Should().NotBeNullOrWhiteSpace();
        }
    }
}
