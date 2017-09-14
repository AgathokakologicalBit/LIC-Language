using System;
using LIC.Parsing;
using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes
{
    public class IfNode : Node
    {
        public ExpressionNode Condition { get; set; }

        public Node TrueBlock { get; set; }
        public Node FalseBlock { get; set; }

        public override void Print(string indent)
        {
            Console.Write("if ");
            Condition.Print(indent);
            Console.Write("\n" + indent + "{\n" + indent + "  ");
            TrueBlock.Print(indent + "  ");
            Console.Write("\n" + indent + "}");

            if (FalseBlock != null)
            {
                Console.Write(" else {\n" + indent);
                FalseBlock.Print(indent + "  ");
                Console.Write(indent + "}");
            }

            Console.WriteLine();
        }
    }
}
