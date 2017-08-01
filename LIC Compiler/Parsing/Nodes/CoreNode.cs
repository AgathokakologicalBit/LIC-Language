using System.Collections.Generic;

namespace LIC_Compiler.Parsing.Nodes
{
    public class CoreNode : Node
    {
        public List<UseNode> UsesNodes;
        public List<ClassNode> ClassNodes;
        public List<FunctionNode> FunctionNodes;

        public CoreNode()
        {
            UsesNodes = new List<UseNode>();
            ClassNodes = new List<ClassNode>();
            FunctionNodes = new List<FunctionNode>();
        }
    }
}
