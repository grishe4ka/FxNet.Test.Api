using FluentAssertions;
using FxNet.Test.Domain.Entities;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Xunit;


namespace FxNet.Test.Tests.Integration
{
    public class TreeTests : TestBase
    {
        public TreeTests(ApiFactory factory) : base(factory) { }

        [Fact]
        public async Task GetTree_ShouldReturn200()
        {
            //await AuthenticateAsync();
            Authenticate();

            //Arrange
            var db = CreateDbContext();
            
            // Seed Trees
            var tree1 = new Tree
            {
                Id = 1,
                Name = "Tree1",
            };

            // Seed TreeNodes
            var root = new TreeNode
            {
                Id = 1,
                Name = "RootNode",
                TreeId = tree1.Id, //1,
                ParentId = null
            };

            var child = new TreeNode
            {
                Id = 2,
                Name = "ChildNode1",
                TreeId = tree1.Id, //1,
                ParentId = root.Id //1
            };

            db.TreeNodes.Add(root);
            db.TreeNodes.Add(child);

            tree1.Nodes.Add(root);
            tree1.Nodes.Add(child);

            db.Trees.Add(tree1);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Trace.WriteLine(ex.ToString());
            }

            //Act
            var response = await _client.PostAsync(
                "/api.user.tree.get?treeName=tree1",
                null
            );

            //Assert
            response.EnsureSuccessStatusCode();

            //response.StatusCode.Should().Be(HttpStatusCode.OK);

            var tree = await response.Content.ReadFromJsonAsync<MNode>();
            tree.Should().NotBeNull();
        }
    }

}
