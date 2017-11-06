using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class FunctionCallNode : ExpressionNode
    {
        public ExpressionNode CalleeExpression { get; set; }
        public List<ExpressionNode> Arguments { get; private set; }

        public FunctionCallNode()
        {
            Arguments = new List<ExpressionNode>(2);
        }
    }
}