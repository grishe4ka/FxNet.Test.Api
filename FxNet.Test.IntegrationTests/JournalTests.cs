using FluentAssertions;
using FxNet.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FxNet.Test.IntegrationTests
{
    public class JournalTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public JournalTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetRange_ReturnsPagedData()
        {
            var response = await _client.PostAsJsonAsync(
                "/api.user.journal.getRange?skip=0&take=10",
                new { search = "error" }
            );

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<MRange_MJournalInfo<MJournalInfo>>();

            result.Should().NotBeNull();
            result!.Items.Should().BeAssignableTo<IEnumerable<MJournalInfo>>();
        }

        [Fact]
        public async Task GetSingle_ReturnsJournalRecord()
        {
            long id = 1;

            var response = await _client.PostAsync($"/api.user.journal.getSingle?id={id}", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<MJournal>();

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
        }
    }

}
