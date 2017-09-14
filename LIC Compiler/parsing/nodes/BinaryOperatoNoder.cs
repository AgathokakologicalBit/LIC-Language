using System;
using LIC.Parsing.Nodes;
using LIC_Compiler.language;

namespace LIC_Compiler.parsing.nodes
{
    public class BinaryOperatorNode : ExpressionNode
    {
        public ExpressionNode LeftOperand { get; set; }
        public ExpressionNode RightOperand { get; set; }
        public Operator Operation { get; set; }

        public BinaryOperatorNode
            (Operator operation, ExpressionNode leftOperand, ExpressionNode rightOperand)
        {
            this.LeftOperand = leftOperand;
            this.RightOperand = rightOperand;
            this.Operation = operation;

            this.Value = this;
        }

        public override void Print(string indent)
        {
            Console.Write("(");
            LeftOperand.Print(indent);
            Console.Write(" " + Operation.Representation + " ");
            RightOperand.Print(indent);
            Console.Write(")");
        }
    }
}
