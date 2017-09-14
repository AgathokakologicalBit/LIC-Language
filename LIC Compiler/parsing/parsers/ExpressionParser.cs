using System;
using LIC.Parsing.Nodes;
using LIC.Tokenization;
using LIC_Compiler.parsing.nodes;

namespace LIC.Parsing.ContextParsers
{
    public static class ExpressionParser
    {
        public static Node Parse(Parser.State state)
        {
            if (state.GetToken().Type != TokenType.Identifier)
            {
                state.ErrorCode = (uint)ErrorCodes.P_IdentifierExpected;
                state.ErrorMessage =
                    $"Identifier expected, but " +
                    $"<{state.GetToken().SubType.ToString()}> were given";
                return null;
            }

            return ParseExpression(state);
        }

        private static Node ParseExpression(Parser.State state)
        {
            switch (state.GetToken().Value)
            {
                case "if": return ParseIfStatement(state);
                case "for": return ParseForStatement(state);
                case "while": return ParseWhileStatement(state);

                case "return": return ParseReturnStatement(state);

                default: return MathExpressionParser.Parse(state);
            }
        }

        private static IfNode ParseIfStatement(Parser.State state)
        {
            state.GetNextNEToken();

            var node = new IfNode
            {
                Condition = MathExpressionParser.Parse(state),
                TrueBlock = CodeParser.Parse(state)
            };

            if (state.GetToken().Is(TokenType.Identifier, "else"))
            {
                node.FalseBlock = CodeParser.Parse(state);
            }

            return node;
        }

        private static ForLoopNode ParseForStatement(Parser.State state)
        {
            throw new NotImplementedException();
        }

        private static WhileLoopNode ParseWhileStatement(Parser.State state)
        {
            throw new NotImplementedException();
        }

        private static ReturnNode ParseReturnStatement(Parser.State state)
        {
            state.GetNextNEToken();
            return new ReturnNode(MathExpressionParser.Parse(state));
        }

        internal static FunctionCallNode ParseFunctionCall(Parser.State state)
        {
            state.GetNextNEToken();

            var call = new FunctionCallNode();
            while (!state.GetToken().Is(TokenSubType.BraceRoundRight))
            {
                var argument = MathExpressionParser.Parse(state);
                call.Arguments.Add(argument);

                if (state.GetToken().Is(TokenSubType.Comma))
                {
                    state.GetNextNEToken();
                }
            }
            state.GetNextNEToken();

            return call;
        }

        internal static ExpressionNode ParseIndexerCall(Parser.State state)
        {
            throw new NotImplementedException();
        }
    }
}
