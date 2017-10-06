using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class ObjectIndexerCallNode : ExpressionNode
    {
        public ExpressionNode CalleeExpression { get; set; }
        public List<ExpressionNode> Arguments { get; private set; }

        public ObjectIndexerCallNode()
        {
            Arguments = new List<ExpressionNode>(2);
        }
    }
}