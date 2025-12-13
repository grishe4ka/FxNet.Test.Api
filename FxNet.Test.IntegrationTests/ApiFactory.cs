using FxNet.Test.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace FxNet.Test.IntegrationTests
{
    public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;

        public ApiFactory()
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("fxnet_test")
                .WithUsername("postgres")
                .WithPassword("1111")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Default"] = _dbContainer.GetConnectionString()
                });
            });
        }

        public async Task InitializeAsync() => await _dbContainer.StartAsync();
        public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
    }

}
