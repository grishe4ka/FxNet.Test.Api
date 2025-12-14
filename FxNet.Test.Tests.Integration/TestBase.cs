using FxNet.Test.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace FxNet.Test.Tests.Integration
{
    public abstract class TestBase(ApiFactory factory) : IClassFixture<ApiFactory>
    {
        protected readonly HttpClient _client = factory.CreateClient();
        //private readonly ApiFactory? _factory;

        protected AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
        protected void ResetDatabase()
        {
            //_factory.CreateScopeWithReset().Dispose();
        }

        protected void Authenticate()
        {
            //var token = GenerateJwtToken();

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJteV91bHRyYV9zZWN1cmVfc2VjcmV0X2tleV9ncmlzaGFfISEhIiwiY29kZSI6Im15X3VsdHJhX3NlY3VyZV9zZWNyZXRfa2V5X2dyaXNoYV8hISEiLCJleHAiOjE3NjU2MTE2NzAsImlzcyI6IkZ4TmV0LlRlc3QiLCJhdWQiOiJGeE5ldC5UZXN0LkNsaWVudCJ9.idfY3JxhofJx0kAEf6-NHKjfcGp8V9o8Gs6SxoWriJE";

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private string GenerateJwtToken()
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("my_ultra_secure_secret_key_grisha_!!!"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                new Claim("userId", "1"),
                new Claim(ClaimTypes.Name, "TestUser")
                },
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
