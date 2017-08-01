using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class CoreNode : Node
    {
        public List<UseNode> UsesNodes { get; private set; }
        public List<ClassNode> ClassNodes { get; private set; }
        public List<FunctionNode> FunctionNodes { get; private set; }

        public CoreNode()
        {
            UsesNodes = new List<UseNode>();
            ClassNodes = new List<ClassNode>();
            FunctionNodes = new List<FunctionNode>();
        }
    }
}
