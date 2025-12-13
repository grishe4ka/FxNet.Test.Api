using FxNet.Test.Domain.Entities;
using FxNet.Test.Domain.Exceptions;
using FxNet.Test.Api;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace FxNet.Test.Tests;

public class TreeNodeDbTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateNode_Success()
    {
        using var db = CreateDbContext();

        var tree = new Tree { Name = "TestTree" };
        db.Trees.Add(tree);
        await db.SaveChangesAsync();

        var rootNode = new TreeNode { TreeId = tree.Id, Name = "root", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(rootNode);
        await db.SaveChangesAsync();

        var child1 = new TreeNode { TreeId = tree.Id, ParentId = rootNode.Id, Name = "child", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(child1);
        await db.SaveChangesAsync();

        Assert.True(db.Trees.Count() == 1);
        Assert.NotNull(db.TreeNodes.FirstOrDefault(n => n.Name == "root"
            && n.TreeId == tree.Id));
        Assert.NotNull(db.TreeNodes.FirstOrDefault(n => n.Name == "child" 
            && n.TreeId == tree.Id && n.ParentId == rootNode.Id));
    }

    [Fact]
    public async Task DeleteNode_Should_DeleteDescendants()
    {
        using var db = CreateDbContext();

        var tree = new Tree { Name = "TestTree" };
        db.Trees.Add(tree);
        await db.SaveChangesAsync();

        var root = new TreeNode { TreeId = tree.Id, Name = "root", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(root);
        await db.SaveChangesAsync();

        var child = new TreeNode { TreeId = tree.Id, ParentId = root.Id, Name = "child", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(child);
        await db.SaveChangesAsync();

        db.TreeNodes.Remove(root);
        await db.SaveChangesAsync();

        Assert.Empty(db.TreeNodes);
    }

    /*
    [Fact]
    public async Task CreateNode_Should_EnforceSiblingNameUniqueness()
    {
        using var db = CreateDbContext();

        var tree = new Tree { Name = "TestTree" };
        db.Trees.Add(tree);
        await db.SaveChangesAsync();

        var rootNode = new TreeNode { TreeId = tree.Id, Name = "root", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(rootNode);
        await db.SaveChangesAsync();

        var child1 = new TreeNode { TreeId = tree.Id, ParentId = rootNode.Id, Name = "child", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(child1);
        await db.SaveChangesAsync();

        var child2 = new TreeNode { TreeId = tree.Id, ParentId = rootNode.Id, Name = "child", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(child2);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var exType = ex.GetType();
        }

        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }
    */

    /*
    [Fact]
    public async Task RenameNode_Should_ThrowOnDuplicateSiblingName()
    {
        using var db = CreateDbContext();

        var tree = new Tree { Name = "TestTree" };
        db.Trees.Add(tree);
        await db.SaveChangesAsync();

        var parent = new TreeNode { TreeId = tree.Id, Name = "root", OwnerId = Guid.NewGuid() };
        db.TreeNodes.Add(parent);
        await db.SaveChangesAsync();

        var n1 = new TreeNode { TreeId = tree.Id, ParentId = parent.Id, Name = "n1", OwnerId = Guid.NewGuid() };
        var n2 = new TreeNode { TreeId = tree.Id, ParentId = parent.Id, Name = "n2", OwnerId = Guid.NewGuid() };
        db.TreeNodes.AddRange(n1, n2);
        await db.SaveChangesAsync();

        n2.Name = "n1";

        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }
    */
}
