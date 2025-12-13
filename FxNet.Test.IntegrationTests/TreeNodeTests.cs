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
    public class TreeNodeTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public TreeNodeTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateNode_AddsNodeToTree()
        {
            string treeName = "tree1";

            var createResponse = await _client.PostAsync(
                $"/api.user.tree.node.create?treeName={treeName}&nodeName=Root",
                null);

            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var treeResponse = await _client.PostAsync($"/api.user.tree.get?treeName={treeName}", null);
            var tree = await treeResponse.Content.ReadFromJsonAsync<MNode>();

            tree!.Children.Should().Contain(x => x.Name == "Root");
        }

        [Fact]
        public async Task RenameNode_ChangesNodeName()
        {
            string treeName = "tree2";

            await _client.PostAsync($"/api.user.tree.node.create?treeName={treeName}&nodeName=OldName", null);

            var tree = await (await _client.PostAsync($"/api.user.tree.get?treeName={treeName}", null))
                .Content.ReadFromJsonAsync<MNode>();

            long nodeId = tree!.Children.First().Id;

            var renameResponse = await _client.PostAsync(
                $"/api.user.tree.node.rename?nodeId={nodeId}&newNodeName=NewName",
                null);

            renameResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedTree = await (await _client.PostAsync($"/api.user.tree.get?treeName={treeName}", null))
                .Content.ReadFromJsonAsync<MNode>();

            updatedTree!.Children.First().Name.Should().Be("NewName");
        }

        [Fact]
        public async Task DeleteNode_RemovesNodeAndChildren()
        {
            string treeName = "tree3";

            await _client.PostAsync($"/api.user.tree.node.create?treeName={treeName}&nodeName=Root", null);

            var tree = await (await _client.PostAsync($"/api.user.tree.get?treeName={treeName}", null))
                .Content.ReadFromJsonAsync<MNode>();

            long nodeId = tree!.Children.First().Id;

            var deleteResponse = await _client.PostAsync(
                $"/api.user.tree.node.delete?nodeId={nodeId}",
                null);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedTree = await (await _client.PostAsync($"/api.user.tree.get?treeName={treeName}", null))
                .Content.ReadFromJsonAsync<MNode>();

            updatedTree!.Children.Should().BeEmpty();
        }


    }
}
