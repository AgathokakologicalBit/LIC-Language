using LIC.Parsing.Nodes;
using LIC.Tokenization;

namespace LIC.Parsing.ContextParsers
{
    public static class FunctionParser
    {
        public static void ParseParametersList(Parser.State state, FunctionNode function)
        {
            while (!state.GetToken().Is(TokenSubType.BraceCurlyRight))
            {
                var paramter = ParseFunctionParameter(state);
                if (paramter == null) { return; }

                function.Parameters.Add(paramter);
                if (state.GetToken().Is(TokenSubType.Comma))
                {
                    state.GetNextNEToken();
                }
            }
            state.GetNextNEToken();
        }

        private static VariableDeclarationNode ParseFunctionParameter(Parser.State state)
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
            TypeNode parameterType = TypeNode.AutoType;

            if (state.GetToken().Is(TokenSubType.Colon))
            {
                state.GetNextNEToken();
                parameterType = TypeParser.Parse(state);
            }
            
            return new VariableDeclarationNode(
                name: parameterName,
                type: parameterType
            );
        }

        public static void ParseTemplateValues(Parser.State state, FunctionNode function)
        {
            // TODO: Add template handling
        }
    }
}
