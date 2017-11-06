namespace LIC.Parsing.Nodes
{
    public class NumberNode : ExpressionNode
    {
        public string NumericValue { get; private set; }

        public bool IsDecimal { get; private set; }


        public NumberNode(string value, bool isDecimal)
        {
            this.NumericValue = value;
            this.IsDecimal = isDecimal;

            this.Value = this;
        }
    }
}
