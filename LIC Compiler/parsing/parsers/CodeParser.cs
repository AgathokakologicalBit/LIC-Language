using LIC.Parsing.Nodes;
using LIC.Tokenization;

namespace LIC.Parsing.ContextParsers
{
    public static class CodeParser
    {
        public static Node Parse(Parser.State state)
        {
            return ParseBlock(state) ?? ExpressionParser.Parse(state);
        }

        public static BlockNode ParseBlock(Parser.State state)
        {
            if (!state.GetToken().Is(TokenSubType.BraceCurlyLeft))
            {
                return null;
            }

            var block = new BlockNode();
            state.GetNextNeToken();
            while (!state.GetToken().Is(TokenSubType.BraceCurlyRight))
            {
                block.Code.Add(ExpressionParser.Parse(state));
                if (state.IsErrorOccured()) { return block; }
            }
            state.GetNextNeToken();

            return block;
        }
    }
}
