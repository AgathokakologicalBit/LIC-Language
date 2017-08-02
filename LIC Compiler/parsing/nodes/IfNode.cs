using LIC.Parsing;
using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes
{
    public class IfNode : Node
    {
        public ExpressionNode Condition { get; set; }

        public Node TrueBlock { get; set; }
        public Node FalseBlock { get; set; }
    }
}
