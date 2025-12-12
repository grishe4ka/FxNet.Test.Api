using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxNet.Test.Contracts
{
    public class MRange<T>
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
