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
                if (state.GetToken().Type != TokenType.Identifier)
                {
                    state.ErrorCode = (uint)ErrorCodes.P_IdentifierExpected;
                    state.ErrorMessage =
                        $"Expected parameter name(<identifier>) " +
                        $"but <{state.GetToken().SubType}>({state.GetToken().Value}) " +
                        $"were given";
                    return;
                }

                var parameterName = state.GetTokenAndMoveNE().Value;

                if (!state.GetToken().Is(TokenSubType.Colon, ":"))
                {
                    state.ErrorCode = (uint)ErrorCodes.P_ColonBeforeTypeSpeceficationNotFound;
                    state.ErrorMessage =
                        $"Expected <Colon>(:) before parameter type specification " +
                        $"but <{state.GetToken().SubType}>({state.GetToken().Value}) were given";
                    return;
                }
                state.GetNextNEToken();

                var parameterType = TypeParser.Parse(state);

                function.Parameters.Add(new VariableNode
                {
                    Name = parameterName,
                    Type = parameterType
                });

                if (state.GetToken().SubType == TokenSubType.Comma)
                {
                    state.GetNextNEToken();
                }
            }

            state.GetNextNEToken();
        }

        public static void ParseTemplateValues(Parser.State state, FunctionNode function)
        {
            // TODO: Add template handling
        }
    }
}
