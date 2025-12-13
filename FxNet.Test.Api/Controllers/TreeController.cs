using FxNet.Test.Model;
using FxNet.Test.Domain.Entities;
using FxNet.Test.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FxNet.Test.Api.Controllers;

[ApiController]
public class TreeController : ControllerBase
{
    private readonly AppDbContext _db;

    public TreeController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("/api.user.tree.get")]
    [Authorize]
    public async Task<MNode?> Get([FromQuery] string treeName)
    {
        if (string.IsNullOrWhiteSpace(treeName))
        {
            throw new SecureValidationException("treeName is required");
        }

        var tree = await _db.Trees
            .Include(t => t.Nodes)
            .FirstOrDefaultAsync(t => t.Name == treeName);

        if (tree == null)
        {
            tree = new Domain.Entities.Tree { Name = treeName };
            _db.Trees.Add(tree);
            await _db.SaveChangesAsync();
        }

        var nodes = await _db.TreeNodes
            .Where(n => n.TreeId == tree.Id)
            .ToListAsync();

        var rootNodes = nodes.Where(n => n.ParentId == null).ToList();
        if (!rootNodes.Any())
        {
            return null;
        }

        MNode Map(Domain.Entities.TreeNode n) => new()
        {
            Id = n.Id,
            Name = n.Name,
            Children = nodes
                .Where(c => c.ParentId == n.Id)
                .Select(Map)
                .ToList()
        };

        return Map(rootNodes.Single());
    }

}
