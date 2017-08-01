using LIC_Compiler.Parsing;
using LIC_Compiler.Tokenization;
using System;
using System.Linq;

namespace LIC_Compiler
{
    public class ErrorHandler
    {
        public static void LogError(State state)
        {
            Console.Error.WriteLine($"Error #LC{state.ErrorCode.ToString("D3")}:");
            Console.Error.WriteLine(state.ErrorMessage);
            Console.Error.WriteLine();

            if (state is Parser.State)
                state.Restore();

            var lastToken =
                state.Tokens.Count > 0
                    ? state.Tokens.Last()
                    : new Token(
                        0, "", 1, 1,
                        Tokenizer.State.Context.Global,
                        TokenType.EOF, TokenSubType.EndOfFile
                    );

            Console.Error.WriteLine($"Line: {lastToken.Line}");
            Console.Error.WriteLine($"Position: {lastToken.Position}");
            Console.Error.WriteLine();

            state.Save();
            state.ErrorCode = 0;

            int codePartBegin = lastToken.Index;
            int codePartEnd = lastToken.Index + lastToken.Length;
            
            state.Restore();
            
            var codePointerString = new String(
                '^',
                Math.Max(1, codePartEnd - codePartBegin)
            );

            Console.Error.WriteLine(
                state.Code
                    .Substring(codePartBegin, Math.Max(1, codePartEnd - codePartBegin))
                    .Replace('\n', ' ') + "\n" +
                codePointerString + "\n"
            );
        }
    }
}
