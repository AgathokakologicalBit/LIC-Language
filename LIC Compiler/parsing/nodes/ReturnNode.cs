namespace LIC.Parsing.Nodes
{
    public class ReturnNode : Node
    {
        public ExpressionNode Expression { get; set; }

        public ReturnNode(ExpressionNode expression)
        {
            this.Expression = expression;
        }
    }
}
