using FluentAssertions;
using FxNet.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FxNet.Test.IntegrationTests
{
    public class AuthTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public AuthTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RememberMe_ReturnsToken()
        {
            //var code = "my_ultra_secure_secret_key_grisha_!!!";
            var code = TestHelper.TokenCode;
            var response = await _client.PostAsync($"/api.user.partner.rememberMe?code={code}", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var token = await response.Content.ReadFromJsonAsync<TokenInfo>();

            token.Should().NotBeNull();
            token!.Token.Should().NotBeNullOrWhiteSpace();
        }

    }

}
