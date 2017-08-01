using LIC_Compiler.Parsing.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIC_Compiler.Parsing.ContextParsers
{
    public static class CodeParser
    {
        public static Node Parse(Parser.State state)
        {
            return ParseBlock(state) ?? MathExpressionParser.Parse(state);
        }

        public static BlockNode ParseBlock(Parser.State state)
        {
            if (!state.GetToken().Is(Tokenization.TokenSubType.BraceCurlyLeft, "{"))
                return null;

            var block = new BlockNode();
            state.GetNextNEToken();
            while (!state.GetToken().Is(Tokenization.TokenSubType.BraceCurlyRight, "}"))
            {
                block.code.Add(ExpressionParser.Parse(state));
                if (state.IsErrorOccured())
                    return block;
            }
            state.GetNextNEToken();

            return block;
        }
    }
}
