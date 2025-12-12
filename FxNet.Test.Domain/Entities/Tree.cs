using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxNet.Test.Domain.Entities
{
    public class Tree
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<TreeNode> Nodes { get; set; } = new List<TreeNode>();
    }
}
