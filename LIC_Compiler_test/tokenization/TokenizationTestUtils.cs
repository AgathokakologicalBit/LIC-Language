using LIC.Tokenization;
using NUnit.Framework;

namespace LIC_Compiler_test.TokenizationTests
{
    public static class TokenizationTestUtils
    {
        public static void TestEof(Tokenizer tokenizer)
        {
            Assert.IsTrue(
                Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, "", 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.EOF, TokenSubType.EndOfFile
                    )
                ),
                "Should correctly tokenize code ending"
            );
        }

        public static bool Match(Token actual, Token expected)
        {
            return actual.Type == expected.Type
                && actual.SubType == expected.SubType
                && actual.Value == expected.Value
                && actual.Context == expected.Context;
        }
    }
}
