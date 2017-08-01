using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class BlockNode : Node
    {
        /// <summary>
        /// Holds list of expressions
        /// </summary>
        public List<Node> code;

        public BlockNode()
        {
            code = new List<Node>();
        }
    }
}
