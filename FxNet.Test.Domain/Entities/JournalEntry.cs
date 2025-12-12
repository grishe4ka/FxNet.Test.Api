using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxNet.Test.Domain.Entities
{
    public class JournalEntry
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Text { get; set; } = null!;

        public string RequestPath { get; set; } = null!;
        public string HttpMethod { get; set; } = null!;
        public string QueryString { get; set; } = null!;
        public string? Body { get; set; }

        public string ExceptionType { get; set; } = null!;
        public string StackTrace { get; set; } = null!;
    }
}
