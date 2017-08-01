using System;

namespace LIC_Compiler.Tokenization.TokenParsing.ParsingModules
{
    public class WhitespaceParser : ITokenParser
    {
        public Token Parse(Tokenizer.State state)
        {
            if (!Char.IsWhiteSpace(state.CurrentCharacter))
                return null;

            char c = state.CurrentCharacter;
            bool isNewLine = c == '\n';
            if (isNewLine)
            {
                state.Line += 1;
                state.LineBegin = state.Index + 1;
            }
            

            Token token = new Token(
                value: c.ToString(),
                
                type: TokenType.Whitespace,
                subType: isNewLine ? TokenSubType.NewLine : TokenSubType.Space
            );

            state.Index += 1;
            return token;
        }
    }
}
