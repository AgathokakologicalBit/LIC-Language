using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class BlockNode : Node
    {
        public List<Node> code;

        public BlockNode()
        {
            code = new List<Node>();
        }
    }
}
