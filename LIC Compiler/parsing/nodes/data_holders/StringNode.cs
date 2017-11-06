namespace LIC.Parsing.Nodes
{
    public class StringNode : ExpressionNode
    {
        public string StringValue { get; set; }

        public StringNode(string value)
        {
            StringValue = value;
            Value = this;
        }
    }
}
