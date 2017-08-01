namespace LIC_Compiler.Tokenization.TokenParsing.ParsingModules
{
    public class CharacterParser : ITokenParser
    {
        public Token Parse(Tokenizer.State state)
        {
            if (state.CurrentCharacter != '\'')
                return null;
            
            state.Index += 1;
            if (state.CurrentCharacter == '\\')
            {
                state.Index += 1;
                return ParseSpecialCharacter(state);
            }

            return ParseNormalCharacter(state);
        }

        public Token ParseNormalCharacter(Tokenizer.State state)
        {
            switch (state.CurrentCharacter)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    state.ErrorCode = (uint)ErrorCodes.T_MisleadingCharacter;
                    state.ErrorMessage =
                        "Character definition is misleading\n" +
                        "Whitespace symbols should be written using escape sequences:\n" +
                        "  \\s - space\n" +
                        "  \\t - tabulation\n" +
                        "  \\n - new line\n" +
                        "  \\r - carriage return";
                    break;
                case '\0':
                    state.ErrorCode = (uint)ErrorCodes.T_UnexpectedEndOfFile;
                    state.ErrorMessage = "Character is expected but 'End of file' was found instead";
                    break;
            }

            return GenerateTokenFor(state.CurrentCharacter, state);
        }

        public Token ParseSpecialCharacter(Tokenizer.State state)
        {
            switch (state.CurrentCharacter)
            {
                case 'n': return GenerateTokenFor('\n', state);
                case 't': return GenerateTokenFor('\t', state);
                case 's': return GenerateTokenFor(' ' , state);
                case 'r': return GenerateTokenFor('\r', state);
            }

            state.ErrorCode = (uint)ErrorCodes.T_SpecialCharacterDoesNotExists;
            state.ErrorMessage = $"Special character identifier '{state.CurrentCharacter}' does not exists";
            return GenerateTokenFor(state.CurrentCharacter, state);
        }

        public Token GenerateTokenFor(char character, Tokenizer.State state)
        {
            state.Index += 1;
            return new Token(
                value: character.ToString(),
                
                type: TokenType.Character,
                subType: TokenSubType.Character
            );
        }
    }
}
