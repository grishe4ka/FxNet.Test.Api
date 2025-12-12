using FxNet.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace FxNet.Test.Api;

public class AppDbContext : DbContext
{
    public DbSet<Tree> Trees => Set<Tree>();
    public DbSet<TreeNode> TreeNodes => Set<TreeNode>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

