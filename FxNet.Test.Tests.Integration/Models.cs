
namespace FxNet.Test.Tests.Integration
{
    public record MJournal(long Id, long EventId, string Text, DateTime CreatedAt);
    public record MJournalInfo(long Id, long EventId, DateTime CreatedAt);
    public record MRange_MJournalInfo(int Skip, int Count, List<MJournalInfo> Items);
    public record MNode(long Id, string Name, List<MNode> Children);
}
