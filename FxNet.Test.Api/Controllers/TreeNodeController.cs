using FxNet.Test.Domain.Entities;
using FxNet.Test.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FxNet.Test.Api.Controllers;

[ApiController]
public class TreeNodeController : ControllerBase
{
    private readonly AppDbContext _db;

    public TreeNodeController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("/api.user.tree.node.create")]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromQuery] string treeName,
        [FromQuery] long? parentNodeId,
        [FromQuery] string nodeName)
    {
        if (string.IsNullOrWhiteSpace(treeName))
            throw new SecureValidationException("treeName is required");
        if (string.IsNullOrWhiteSpace(nodeName))
            throw new SecureValidationException("nodeName is required");

        var tree = await _db.Trees.FirstOrDefaultAsync(t => t.Name == treeName);
        if (tree == null)
        {
            tree = new Tree { Name = treeName };
            _db.Trees.Add(tree);
            await _db.SaveChangesAsync();
        }

        TreeNode? parent = null;
        if (parentNodeId.HasValue)
        {
            parent = await _db.TreeNodes
                .FirstOrDefaultAsync(n => n.Id == parentNodeId.Value && n.TreeId == tree.Id);

            if (parent == null)
                throw new SecureException("Parent node does not belong to this tree");
        }

        var exists = await _db.TreeNodes
            .AnyAsync(n => n.TreeId == tree.Id
                           && n.ParentId == parentNodeId
                           && n.Name == nodeName);

        if (exists)
            throw new SecureValidationException("Node name must be unique among siblings");

        var newNode = new TreeNode
        {
            TreeId = tree.Id,
            ParentId = parentNodeId,
            Name = nodeName,
            OwnerId = Guid.NewGuid()
        };

        _db.TreeNodes.Add(newNode);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("/api.user.tree.node.delete")]
    [Authorize]
    public async Task<IActionResult> Delete([FromQuery] long nodeId)
    {
        var node = await _db.TreeNodes.FindAsync(nodeId);
        if (node == null)
            throw new SecureException("Node not found");

        _db.TreeNodes.Remove(node);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("/api.user.tree.node.rename")]
    [Authorize]
    public async Task<IActionResult> Rename(
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName)
    {
        if (string.IsNullOrWhiteSpace(newNodeName))
            throw new SecureValidationException("newNodeName is required");

        var node = await _db.TreeNodes.FindAsync(nodeId);
        if (node == null)
            throw new SecureException("Node not found");

        var exists = await _db.TreeNodes.AnyAsync(n =>
            n.TreeId == node.TreeId &&
            n.ParentId == node.ParentId &&
            n.Name == newNodeName &&
            n.Id != node.Id);

        if (exists)
            throw new SecureValidationException("Node name must be unique among siblings");

        node.Name = newNodeName;
        await _db.SaveChangesAsync();

        return Ok();
    }
}
