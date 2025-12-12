using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxNet.Test.Domain.Entities
{
    public class TreeNode
    {
        public long Id { get; set; }

        public long TreeId { get; set; }
        public Tree Tree { get; set; } = null!;

        public long? ParentId { get; set; }
        public TreeNode? Parent { get; set; }

        public ICollection<TreeNode> Children { get; set; } = new List<TreeNode>();

        public string Name { get; set; } = null!;

        // Пример поля для независимости деревьев (например, владелец дерева)
        public Guid OwnerId { get; set; }
    }
}
