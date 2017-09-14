using System;
using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes.data_holders
{
    public class StringNode : ExpressionNode
    {
        public string StringValue { get; set; }

        public StringNode(string value)
        {
            this.StringValue = value;
            this.Value = this;
        }

        public override void Print(string indent)
        {
            Console.Write('"' + StringValue + '"');
        }
    }
}
