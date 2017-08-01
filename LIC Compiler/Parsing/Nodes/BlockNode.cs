using System.Collections.Generic;

namespace LIC_Compiler.Parsing.Nodes
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
