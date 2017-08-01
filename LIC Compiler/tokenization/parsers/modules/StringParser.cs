using System.Linq;

namespace LIC_Compiler.Tokenization.TokenParsing.ParsingModules
{
    public class StringParser : ITokenParser
    {
        public Token Parse(Tokenizer.State state)
        {
            if (state.CurrentCharacter != '"')
                return null;

            int begin = state.Index + 1;

            do
            {
                state.Index += 1;
                while (state.CurrentCharacter == '\\')
                    state.Index += 2;
            } while (!"\"\0".Contains(state.CurrentCharacter));

            if (state.CurrentCharacter == '\0')
            {
                state.Index -= 1;
                state.ErrorCode = (uint)ErrorCodes.T_UnexpectedEndOfFile;
                state.ErrorMessage = "End of string was not found";
            }

            return new Token(
                value: state.Code.Substring(begin, state.Index++ - begin),

                type: TokenType.String,
                subType: TokenSubType.String
            );
        }
    }
}
