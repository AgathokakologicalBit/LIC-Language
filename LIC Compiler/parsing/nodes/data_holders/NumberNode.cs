using System;
using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes.data_holders
{
    public class NumberNode : ExpressionNode
    {
        public string NumericValue { get; private set; }

        public bool IsDecimal { get; private set; }
        
        
        public NumberNode(string value, bool isDecimal)
        {
            this.NumericValue = value;
            this.IsDecimal = IsDecimal;

            this.Value = this;
        }

        public override void Print(string indent)
        {
            Console.Write(NumericValue);
        }
    }
}
