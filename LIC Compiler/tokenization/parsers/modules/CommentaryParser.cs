using System.Linq;

namespace LIC.Tokenization.TokenParsing.ParsingModules
{
    public class CommentaryParser : ITokenParser
    {
        public Token Parse(Tokenizer.State state)
        {
            if (state.CurrentCharacter == '/')
            {
                int begin = state.Index;
                int line = state.Line;
                int position = state.Position;

                bool inline = false;

                state.Index += 1;
                if (state.CurrentCharacter == '/')
                {
                    state.Index += 1;
                    ParseInlineCommentary(state);
                    inline = true;
                }
                else if (state.CurrentCharacter == '*')
                {
                    state.Index += 1;
                    ParseMultilineCommentary(state);
                }
                else return null;

                if (state.IsErrorOccured())
                {
                    state.Index = begin + 2;
                }

                return new Token(
                    value: state.Code.Substring(begin, state.Index - begin),
                    
                    type: TokenType.Commentary,
                    subType: inline
                        ? TokenSubType.InlineCommentary
                        : TokenSubType.MultilineCommentary
                );
            }

            return null;
        }
        
        private void ParseInlineCommentary(Tokenizer.State state)
        {
            while (state.CurrentCharacter != '\0'
                    && !"\r\n".Contains(state.CurrentCharacter))
                state.Index += 1;
        }

        private void ParseMultilineCommentary(Tokenizer.State state)
        {
            while (true)
            {
                if (state.CurrentCharacter == '\0')
                {
                    state.ErrorCode = (uint)ErrorCodes.T_UnexpectedEndOfFile;
                    state.ErrorMessage = "Commentary was not closed";
                    return;
                }

                if (state.CurrentCharacter == '*')
                {
                    state.Index += 1;
                    if (state.CurrentCharacter == '/')
                    {
                        state.Index += 1;
                        return;
                    }
                }

                if (state.CurrentCharacter == '\n')
                {
                    state.Line += 1;
                    state.LineBegin = state.Index + 1;
                }

                state.Index += 1;
            }
        }
    }
}
