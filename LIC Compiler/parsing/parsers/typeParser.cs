using LIC.Parsing.Nodes;
using LIC.Tokenization;
using System.Text;

namespace LIC.Parsing.ContextParsers
{
    public static class TypeParser
    {
        public static TypeNode Parse(Parser.State state)
        {
            state.Save();
            TypeNode type = new TypeNode();

            while (true)
            {
                var tok = state.GetToken();
                if (tok.Is(TokenType.Identifier, "const"))
                {
                    if (type.IsConstant)
                    {
                        state.ErrorCode = (uint)ErrorCodes.P_DuplicatedModifier;
                        state.ErrorMessage =
                            "<const> type modifier can not be used twice" +
                            " for the same type";
                        return null;
                    }

                    type.IsConstant = true;
                    state.GetNextNEToken();
                    continue;
                }
                else if (tok.Is(TokenType.Identifier, "ref"))
                {
                    state.GetNextNEToken();
                    type.IsReference = true;
                    type.ReferenceType = Parse(state);

                    if (type.IsConstant)
                    {
                        state.ErrorCode = (uint)ErrorCodes.P_ReferenceCanNotBeConstant;
                        state.ErrorMessage =
                            "Reference can not have a constant modifier:\n" +
                            type.ToString();
                    }

                    return type;
                }
                else if (tok.Is(TokenSubType.BraceSquareLeft))
                {
                    state.GetNextNEToken();
                    type.IsArrayType = true;

                    // TODO: Parse sizes
                    if (!state.GetToken().Is(TokenSubType.BraceSquareRight))
                    {
                        state.ErrorCode = 999;
                        state.ErrorMessage =
                            "Array does not support sizes and dimmensions " +
                            "in the current version";
                        return type;
                    }
                    state.GetNextNEToken();

                    type.ReferenceType = Parse(state);

                    if (type.ReferenceType.IsReference)
                    {
                        state.ErrorCode = (uint)ErrorCodes.P_ArrayCanNotContainReferences;
                        state.ErrorMessage =
                            "Array can not consist of references";
                    }

                    return type;
                }

                type.TypePath = ParsePath(state);
                return type;
            }
        }

        public static string ParsePath(Parser.State state)
        {
            StringBuilder pathBuilder = new StringBuilder();

            while (true)
            {
                if (state.GetToken().Type != TokenType.Identifier)
                {
                    state.ErrorCode = (uint)ErrorCodes.P_IdentifierExpected;
                    state.ErrorMessage =
                        $"Expected path name(<identifier>) " +
                        $"but <{state.GetToken().SubType.ToString()}> " +
                        $"were given";
                    return null;
                }

                pathBuilder.Append(state.GetToken().Value);
                if (state.GetNextNEToken().SubType == TokenSubType.Colon)
                {
                    pathBuilder.Append(":");
                    state.GetNextNEToken();
                    continue;
                }

                break;
            }

            return pathBuilder.ToString();
        }
    }
}
