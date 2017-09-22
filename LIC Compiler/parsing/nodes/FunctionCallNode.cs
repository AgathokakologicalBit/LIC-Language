using LIC.Parsing.Nodes;
using System.Collections.Generic;

namespace LIC_Compiler.parsing.nodes
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