using System;

namespace LIC.Tokenization.TokenParsing.ParsingModules
{
    public static class CompilerDirectiveParser
    {
        public static Token Parse(Tokenizer.State state)
        {
            if (state.CurrentCharacter != '#') { return null; }

            var index = state.Index;

            state.Index += 1;
            while (IsMatching(state.CurrentCharacter, state))
            {
                state.Index += 1;
            }

            var length = state.Index - index;
            if (length == 1)
            {
                state.ErrorCode = (uint)ErrorCodes.T_CompilerDirectiveNameIsNotStated;
                state.ErrorMessage = "Compiler directive name is not stated";
            }

            return new Token(
                value: state.Code.Substring(index, length),

                type: TokenType.CompilerDirective,
                subType: TokenSubType.CompilerDirective
            );
        }

        private static bool IsMatching(char c, Tokenizer.State state)
        {
            if (Char.IsLetter(c))
            {
                if (c > 127)
                {
                    // If compiler directive name contains non-ascii letters
                    // it will be written as error to state
                    state.ErrorCode = (uint)ErrorCodes.T_InvalidCompilerDirectiveName;
                    state.ErrorMessage =
                        "Compiler directive name should consist of:\n" +
                        "  - latin letters\n" +
                        "  - '_' symbols";
                }

                return true;
            }

            return c == '_';
        }
    }
}
