
using FxNet.Test.Api;
using FxNet.Test.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

namespace FxNet.Test.Tests.Integration
{
    public class ApiFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var key = Encoding.UTF8.GetBytes("my_ultra_secure_secret_key_grisha_!!!");

            builder.ConfigureServices(services =>
            {
                // Удаляем реальный DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Добавляем InMemory DB
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Выполняем SEED данных
                /*
                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                */

                var db = CreateDbContext();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                //SeedDatabase(db);
            });

            return base.CreateHost(builder);
        }

        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private void SeedDatabase(AppDbContext db)
        {
            // Seed Journal
            db.JournalEntries.AddRange(
                new JournalEntry
                {
                    Id = 1,
                    EventId = 100,
                    Text = "Test event 1",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new JournalEntry
                {
                    Id = 2,
                    EventId = 200,
                    Text = "Test event 2",
                    CreatedAt = DateTime.UtcNow
                }
            );
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Trace.WriteLine(ex.ToString());
            }
            // Seed Trees
            var tree1 = new Tree
            {
                //Id = 1,
                Name = "Tree1",
            };

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

            // Seed TreeNodes
            var root = new TreeNode
            {
                //Id = 1,
                Name = "RootNode",
                TreeId = tree1.Id, //1,
                ParentId = null
            };

            var child = new TreeNode
            {
                //Id = 2,
                Name = "ChildNode1",
                TreeId = tree1.Id, //1,
                ParentId = root.Id //1
            };

            db.TreeNodes.Add(root);
            db.TreeNodes.Add(child);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}