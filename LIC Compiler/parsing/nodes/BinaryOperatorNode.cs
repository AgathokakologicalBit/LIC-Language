using LIC.Language;

namespace LIC.Parsing.Nodes
{
    public class BinaryOperatorNode : ExpressionNode
    {
        public ExpressionNode LeftOperand { get; set; }
        public ExpressionNode RightOperand { get; set; }
        public Operator Operation { get; set; }

        public BinaryOperatorNode
            (Operator operation, ExpressionNode leftOperand, ExpressionNode rightOperand)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Operation = operation;

            Value = this;
        }
    }
}
