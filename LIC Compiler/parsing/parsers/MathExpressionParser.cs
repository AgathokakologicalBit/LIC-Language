using System;
using LIC.Parsing.Nodes;
using LIC_Compiler.parsing.nodes;
using LIC_Compiler.language;
using LIC.Tokenization;

namespace LIC.Parsing.ContextParsers
{
    public static class MathExpressionParser
    {
        public static ExpressionNode Parse(Parser.State state)
        {
            ExpressionNode leftOperand = ParseUnit(state);
            state.GetNextNEToken();
            return Parse(state, leftOperand);
        }

        private static ExpressionNode Parse
            (Parser.State state, ExpressionNode leftOperand, uint basePriority = 0)
        {
            Operator operation = GetOperator(state.GetToken());
            if (operation.Equals(OperatorList.Unknown)) { return leftOperand; };

            state.GetNextNEToken();
            ExpressionNode rightOperand = ParseUnit(state);
            state.GetNextNEToken();

            // If operator is rigth-sided(like equal sign(=)) then parse block after it first.
            if (operation.IsRightSided)
            {
                return new BinaryOperatorNode(
                    operation,
                    leftOperand,
                    Parse(state, rightOperand)
                );
            }

            // Get next operator
            var nextOperation = GetOperator(state.GetToken());
            state.GetNextNEToken();

            while (
                !nextOperation.Equals(OperatorList.Unknown)
                && nextOperation.Priority > basePriority
            )
            {
                if (nextOperation.Priority > operation.Priority)
                {
                    rightOperand = Parse(state, rightOperand, operation.Priority);

                    leftOperand = new BinaryOperatorNode(operation, leftOperand, rightOperand);
                    if (state.GetNextNEToken().Type == TokenType.Identifier)
                    {
                        return leftOperand;
                    }

                    operation = GetOperator(state.GetToken());
                    if (operation.Equals(OperatorList.Unknown))
                    {
                        return leftOperand;
                    }
                    state.GetNextNEToken();
                }
                else
                {
                    leftOperand = new BinaryOperatorNode(operation, leftOperand, rightOperand);
                    operation = nextOperation;
                }

                rightOperand = ParseUnit(state);
                nextOperation = GetOperator(state.GetNextNEToken());
            }

            return new BinaryOperatorNode(operation, leftOperand, rightOperand);
        }

        private static Operator GetOperator(Token operatorToken)
        {
            throw new NotImplementedException();
        }

        private static ExpressionNode ParseUnit(Parser.State state)
        {
            throw new NotImplementedException();
        }
    }
}
