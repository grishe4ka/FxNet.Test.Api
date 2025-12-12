
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace FxNet.Test.DataSeeder
{
    /*
    http://localhost:5666/api.user.tree.node.create?treeName=tName&parentNodeId=pNodeId&nodeName=nName";
    */

    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Data Seeder Started.");

            var login = "1111";
            var pass = "1111";
            var code = "my_ultra_secure_secret_key_grisha_!!!";
            var token = GetToken(login, pass, code);

            Console.WriteLine(token);

            var baseUri = "http://localhost:5666/api.user.tree.node.create?treeName=tName&parentNodeId=pNodeId&nodeName=nName";

            var client = new HttpClient { BaseAddress = new Uri(baseUri) };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            int count = 1;

            for (int i = 0; i < count; i++)
            {
                var requestString = baseUri
                    .Replace("tName", $"tree_{i}")
                    .Replace("nName", $"node_{i}");
                Console.WriteLine(requestString);
                var requestUri = new Uri(requestString);
                try
                {
                    var response = await client.PostAsJsonAsync(requestUri, new object());
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            Console.WriteLine($"{count} trees added to DataBase. Press Any Key.");
            Console.ReadKey();
        }

        /*
        private static async Task<string> GetToken2(string login, string pass, string key)
        {
            var baseUri = "http://localhost:5666/api/Auth/token";
            var client = new HttpClient { BaseAddress = new Uri(baseUri) };
            var content = new TokenRequest(login, pass, key);
            try
            {
                var response = await client.PostAsJsonAsync(baseUri, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "token not received";
        }
        */

        //[HttpPost("/api.user.partner.rememberMe")]
        public static string GetToken(string login, string pass, string code)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(code));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, code),
                new Claim("code", code)
            };

            var token = new JwtSecurityToken(
                issuer: "FxNet.Test",
                audience: "FxNet.Test.Client",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
        public record TokenRequest(string Username, string Password, string key);
    }
}
