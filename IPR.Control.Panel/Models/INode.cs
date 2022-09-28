using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.Control.Panel.Models
{
    public interface INode
    {
        string Name { get; }
        bool IsExpanded { get; }
        IEnumerable<INode> Children { get; }
    }
}
