namespace LIC.Parsing.Nodes
{
    public class ForLoopNode : Node
    {
        public ExpressionNode Condition { get; set; }

        public Node CodeBlock { get; set; }
    }
}
