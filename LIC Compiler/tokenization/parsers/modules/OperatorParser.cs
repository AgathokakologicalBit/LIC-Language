namespace LIC.Tokenization.TokenParsing.ParsingModules
{
    public static class OperatorParser
    {
        public static Token Parse(Tokenizer.State state)
        {
            var st = GetTypeFor(state.CurrentCharacter);

            if (st == TokenSubType.Unknown) { return null; }

            var t = new Token(
                value: state.CurrentCharacter.ToString(),

                type: IsMathOperator(state.CurrentCharacter)
                        ? TokenType.MathOperator
                        : TokenType.SpecialOperator,
                subType: st
            );

            state.Index += 1;
            return t;
        }

        private static TokenSubType GetTypeFor(char c)
        {
            switch (c)
            {
                case '=': return TokenSubType.Equal;
                case '+': return TokenSubType.Plus;
                case '-': return TokenSubType.Dash;
                case '*': return TokenSubType.Star;
                case '/': return TokenSubType.Slash;
                case '\\': return TokenSubType.Backslash;
                case '^': return TokenSubType.Caret;
                case '%': return TokenSubType.Percent;

                case '.': return TokenSubType.Dot;
                case ',': return TokenSubType.Comma;

                case '|': return TokenSubType.VerticalBar;
                case '&': return TokenSubType.Ampersand;

                case '?': return TokenSubType.QuestionMark;
                case '!': return TokenSubType.ExclamationMark;


                case '@': return TokenSubType.AtSign;

                case ':': return TokenSubType.Colon;
                case ';': return TokenSubType.SemiColon;


                case '(': return TokenSubType.BraceRoundLeft;
                case ')': return TokenSubType.BraceRoundRight;

                case '<': return TokenSubType.BraceTriangularLeft;
                case '>': return TokenSubType.BraceTriangularRight;

                case '[': return TokenSubType.BraceSquareLeft;
                case ']': return TokenSubType.BraceSquareRight;

                case '{': return TokenSubType.BraceCurlyLeft;
                case '}': return TokenSubType.BraceCurlyRight;


                default: return TokenSubType.Unknown;
            }
        }

        private static bool IsMathOperator(char c)
        {
            switch (c)
            {
                case '+': case '-':
                case '*': case '/': case '%':
                case '^':
                case '|': case '&':
                case '=': case '<': case '>':
                    return true;

                default:
                    return false;
            }
        }
    }
}
