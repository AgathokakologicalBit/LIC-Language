using LIC.Parsing.Nodes;
using System;
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

        public override void Print(string indent)
        {
            CalleeExpression.Print(indent);
            Console.Write("(");
            for (int i = 0; i < Arguments.Count; ++i)
            {
                if (i != 0) Console.Write(", ");
                Arguments[i].Print(indent);
            }
            Console.Write(")");
        }
    }
}