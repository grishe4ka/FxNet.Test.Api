

namespace FxNet.Test.Contracts
{
    public class MJournal
    {
        public string Text { get; set; } = null!;
        public long Id { get; set; }
        public long EventId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
