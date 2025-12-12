using FxNet.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FxNet.Test.Api.Configurations;

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.HasKey(j => j.Id);
        builder.Property(j => j.EventId).IsRequired();
        builder.Property(j => j.Text).IsRequired();
        builder.Property(j => j.CreatedAt).IsRequired();
        builder.Property(j => j.ExceptionType).IsRequired();
        builder.Property(j => j.StackTrace).IsRequired();
    }
}

