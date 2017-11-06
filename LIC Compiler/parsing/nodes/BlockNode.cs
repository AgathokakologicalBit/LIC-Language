using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class BlockNode : Node
    {
        /// <summary>
        /// Holds list of expressions
        /// </summary>
        public List<Node> Code { get; private set; }

        public BlockNode()
        {
            Code = new List<Node>();
        }
    }
}
