using FluentAssertions;
using FxNet.Test.Tests.Integration;
using System.Net;
using Xunit;

namespace FxNet.Test.Tests.Integration
{
    public class TreeNodeTests : TestBase
    {
        public TreeNodeTests(ApiFactory factory) : base(factory) { }

        [Fact]
        public async Task CreateNode_ShouldReturn200()
        {
            //await AuthenticateAsync();
            Authenticate();

            var response = await _client.PostAsync(
                "/api.user.tree.node.create?treeName=tree1&nodeName=RootNode",
                null
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RenameNode_ShouldReturn200()
        {
            //await AuthenticateAsync();
            Authenticate();

            var response = await _client.PostAsync(
                "/api.user.tree.node.rename?nodeId=1&newNodeName=Renamed",
                null
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteNode_ShouldReturn200()
        {
            //await AuthenticateAsync();
            Authenticate();

            var response = await _client.PostAsync(
                "/api.user.tree.node.delete?nodeId=1",
                null
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}