using System;

namespace LIC_Compiler.Tokenization.TokenParsing.ParsingModules
{
    public class NumberParser : ITokenParser
    {
        public Token Parse(Tokenizer.State state)
        {
            Tokenizer.State s = new Tokenizer.State(state);

            ParseNumber(state, out int decimalPoint);

            if (state.Index == s.Index)
                return null;

            return new Token(
                value: state.Code.Substring(s.Index, state.Index - s.Index),
                
                type: TokenType.Number,
                subType: decimalPoint >= 0
                            ? TokenSubType.Decimal
                            : TokenSubType.Integer
            );
        }

        private void ParseNumber(Tokenizer.State state, out int decimalPoint)
        {
            decimalPoint = -2;

            while (IsMatching(state, ref decimalPoint))
                state.Index += 1;

            if (decimalPoint == state.Index - 1)
            {
                decimalPoint = -2;
                state.Index -= 1;
            }
        }

        private bool IsMatching(Tokenizer.State state, ref int decimalPoint)
        {
            if (Char.IsDigit(state.CurrentCharacter))
                return true;

            return IsMatchingDecimal(state, ref decimalPoint);
        }

        private bool IsMatchingDecimal(Tokenizer.State state, ref int decimalPoint)
        {
            if (state.CurrentCharacter != '.'
                || decimalPoint >= 0)
                return false;

            decimalPoint = state.Index;
            return true;
        }
    }
}
