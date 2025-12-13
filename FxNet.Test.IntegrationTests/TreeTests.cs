using FluentAssertions;
using FxNet.Test.IntegrationTests;
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
    public class TreeTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public TreeTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTree_CreatesTreeIfNotExists()
        {
            string treeName = "myTree";

            var response = await _client.PostAsync($"/api.user.tree.get?treeName={treeName}", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var tree = await response.Content.ReadFromJsonAsync<MNode>();

            tree.Should().NotBeNull();
            tree!.Name.Should().Be(treeName);
        }

        [Fact]
        public async Task AuthenticatedRequest_RequiresToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api.user.tree.get?treeName=test");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }

}
