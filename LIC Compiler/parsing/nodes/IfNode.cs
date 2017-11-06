namespace LIC.Parsing.Nodes
{
    public class IfNode : Node
    {
        public ExpressionNode Condition { get; set; }

        public Node TrueBlock { get; set; }
        public Node FalseBlock { get; set; }
    }
}
