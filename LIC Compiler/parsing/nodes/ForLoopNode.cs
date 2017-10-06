using LIC.Parsing;
using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes
{
    public class ForLoopNode : Node
    {
        public ExpressionNode Condition { get; set; }

        public Node CodeBlock { get; set; }
    }
}
