using LIC.Parsing.Nodes;
using LIC.Tokenization;

namespace LIC.Parsing.ContextParsers
{
    public static class FunctionParser
    {
        public static void ParseParametersList(Parser.State state, FunctionNode function)
        {
            while (!state.GetToken().Is(TokenSubType.BraceRoundRight, ")"))
            {
                var paramter = ParseFunctionParameter(state);
                if (paramter == null) { return; }

                function.Parameters.Add(paramter);
                if (!AssertTokenIsCommaOrBrace(state.GetTokenAndMoveNE()))
                {
                    return;
                }
            }
        }

        private static bool AssertTokenIsCommaOrBrace(Token token)
        {
            return token.SubType == TokenSubType.Comma
                || token.SubType == TokenSubType.BraceRoundRight;
        }

        private static VariableNode ParseFunctionParameter(Parser.State state)
        {
            if (state.GetToken().Type != TokenType.Identifier)
            {
                state.ErrorCode = (uint)ErrorCodes.P_IdentifierExpected;
                state.ErrorMessage =
                    $"Expected parameter name(<identifier>) " +
                    $"but <{state.GetToken().SubType}>({state.GetToken().Value}) " +
                    $"were given";
                return null;
            }

            var parameterName = state.GetTokenAndMoveNE().Value;

            if (!state.GetToken().Is(TokenSubType.Colon, ":"))
            {
                state.ErrorCode = (uint)ErrorCodes.P_ColonBeforeTypeSpeceficationNotFound;
                state.ErrorMessage =
                    $"Expected <Colon>(:) before parameter type specification " +
                    $"but <{state.GetToken().SubType}>({state.GetToken().Value}) were given";
                return null;
            }
            state.GetNextNEToken();

            var parameterType = TypeParser.Parse(state);
            return new VariableNode
            {
                Name = parameterName,
                Type = parameterType
            };
        }

        public static void ParseTemplateValues(Parser.State state, FunctionNode function)
        {
            // TODO: Add template handling
        }
    }
}
