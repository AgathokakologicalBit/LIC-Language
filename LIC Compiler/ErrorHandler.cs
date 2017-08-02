using LIC.Parsing;
using LIC.Tokenization;
using System;
using System.Linq;

namespace LIC
{
    public static class ErrorHandler
    {
        public static void LogError(State state)
        {
            LogErrorInfo(state);
            if (state is Parser.State) { state.Restore(); }

            var lastToken = GetLastTokenOrEOF(state);
            LogErrorPosition(lastToken);
            
            var codePointerString = new String(
                '^',
                Math.Max(1, lastToken.Length)
            );

            LogCodePart(state, codePointerString, lastToken.Index);
        }

        public static void LogErrorInfo(State state)
        {
            Console.Error.WriteLine(
                $"Error #LC{state.ErrorCode.ToString("D3")}:\n{state.ErrorMessage}\n"
            );
        }

        public static void LogErrorPosition(Token token)
        {
            Console.Error.WriteLine(
                $"Line: {token.Line}\nPosition: {token.Position}\n"
            );
        }

        private static void LogCodePart(State state, string underline, int from)
        {
            Console.Error.WriteLine(
                state.Code
                    .Substring(from, underline.Length)
                    .Replace('\n', ' ') +
                    "\n" + underline + "\n"
            );
        }

        private static Token GetLastTokenOrEOF(State state)
        {
            return state.Tokens.LastOrDefault() ?? new Token(
                0, "", 1, 1,
                Tokenizer.State.Context.Global,
                TokenType.EOF, TokenSubType.EndOfFile
            );
        }
    }
}
