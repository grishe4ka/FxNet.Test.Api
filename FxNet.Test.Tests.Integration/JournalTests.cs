using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FxNet.Test.Tests.Integration
{
    public class JournalTests : TestBase
    {
        public JournalTests(ApiFactory factory) : base(factory) { }

        [Fact]
        public async Task GetRange_ShouldReturn200()
        {
            //await AuthenticateAsync();
            Authenticate();

            var response = await _client.PostAsync(
                "/api.user.journal.getRange?skip=0&take=10",
                JsonContent.Create(new { }) // пустой filter
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<MRange_MJournalInfo>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSingle_ShouldReturn200()
        {
            //await AuthenticateAsync();
            Authenticate();

            var response = await _client.PostAsync(
                "/api.user.journal.getSingle?id=1",
                null
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<MJournal>();
            result.Should().NotBeNull();
        }
    }

}
