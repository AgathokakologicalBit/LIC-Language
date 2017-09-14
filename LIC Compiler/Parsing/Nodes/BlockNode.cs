using System;
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

        public override void Print(string indent)
        {
            foreach (var node in Code) {
                Console.Write(indent);
                node.Print(indent);
                Console.WriteLine();
            }
        }
    }
}
