using FxNet.Test.Model;
using System.Net.Http.Headers;
using System.Text.Json;


namespace FxNet.Test.Tests.Integration
{
    public abstract class IntegrationTestBase3 : IClassFixture<ApiFactory>
    {
        protected readonly HttpClient Client;

        protected IntegrationTestBase3(ApiFactory factory)
        {
            Client = factory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            var response = await Client.PostAsync(
                "/api.user.partner.rememberMe?code=my_ultra_secure_secret_key_grisha_!!!",
                null);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenInfo>(json)?.Token;

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

    }

}
