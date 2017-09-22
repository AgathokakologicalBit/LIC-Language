using LIC.Parsing;
using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes
{
    public class ReturnNode : Node
    {
        public ExpressionNode Expression { get; set; }

        public ReturnNode(ExpressionNode expression)
        {
            this.Expression = expression;
        }
    }
}
