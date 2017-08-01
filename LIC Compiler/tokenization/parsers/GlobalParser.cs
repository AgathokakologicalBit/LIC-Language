using LIC_Compiler.Tokenization.TokenParsing.ParsingModules;
using System.Collections.Generic;

namespace LIC_Compiler.Tokenization.TokenParsing
{
    public static class GlobalParser
    {
        public static List<ITokenParser> parsers = new List<ITokenParser>() {
            new WhitespaceParser(),
            new CommentaryParser(),

            new OperatorParser(),

            new CompilerDirectiveParser(),

            new NumberParser(),
            new IdentifierParser(),

            new CharacterParser(),
            new StringParser(),
        };

        public static Token Parse(Tokenizer.State state)
        {
            foreach (ITokenParser parser in parsers)
                if (TryParse(state, parser, out Token token))
                    return token;

            return null;
        }

        private static bool TryParse
            (Tokenizer.State state, ITokenParser parser, out Token token)
        {
            state.Save();
            Tokenizer.State stateCopy = new Tokenizer.State(state);

            token = parser.Parse(state);

            if (state.IsErrorOccured())
            {
                state.Drop();
                return true;
            }
            else if (token != null)
            {
                token.Index = stateCopy.Index;
                token.Line = stateCopy.Line;
                token.Position = stateCopy.Position;

                state.Drop();
                return true;
            }

            state.Restore();
            return false;
        }
    }
}
