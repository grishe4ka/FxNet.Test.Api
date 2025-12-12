using FxNet.Test.Contracts;
using FxNet.Test.Domain.Exceptions;
using FxNet.Test.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FxNet.Test.Api.Controllers;

[ApiController]
public class JournalController : ControllerBase
{
    private readonly AppDbContext _db;

    public JournalController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("/api.user.journal.getRange")]
    [Authorize]
    public async Task<MRange<MJournalInfo>> GetRange(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromBody] MJournalFilter? filter)
    {
        var query = _db.JournalEntries.AsQueryable();

        if (filter?.From is not null)
            query = query.Where(j => j.CreatedAt >= filter.From.Value);

        if (filter?.To is not null)
            query = query.Where(j => j.CreatedAt <= filter.To.Value);

        if (!string.IsNullOrEmpty(filter?.Search))
            query = query.Where(j => j.Text.Contains(filter.Search));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(j => new MJournalInfo
            {
                Id = j.Id,
                EventId = j.EventId,
                CreatedAt = j.CreatedAt
            })
            .ToListAsync();

        return new MRange<MJournalInfo>
        {
            Skip = skip,
            Count = total,
            Items = items
        };
    }

    [HttpPost("/api.user.journal.getSingle")]
    [Authorize]
    public async Task<MJournal> GetSingle([FromQuery] long id)
    {
        var j = await _db.JournalEntries.FindAsync(id);
        if (j == null)
            throw new SecureException($"Journal entry {id} not found");

        return new MJournal
        {
            Id = j.Id,
            EventId = j.EventId,
            CreatedAt = j.CreatedAt,
            Text = j.Text
        };
    }
}
