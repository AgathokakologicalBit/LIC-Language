using LIC;
using LIC.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void TestString_0_Basic()
        {
            TestString("!_test$");
        }

        [TestMethod]
        public void TestString_1_Empty()
        {
            TestString("");
        }

        [TestMethod]
        public void TestString_2_SpecialCharacters()
        {
            TestString("\\r\\n\\t\\\"");
        }

        [TestMethod]
        public void TestString_3_NotClosed()
        {
            var code = "\"testing string";
            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            tokenizer.GetNextToken();

            Assert.IsTrue(
                tokenizer.state.IsErrorOccured(),
                "Should return an error if string does not ends"
            );
            Assert.AreEqual(
                tokenizer.state.ErrorCode,
                (uint)ErrorCodes.T_UnexpectedEndOfFile,
                "Should have right error type"
            );
        }

        private static void TestString(string code)
        {
            var tokenizer = new Tokenizer($"\"{code}\"", new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, code, 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.String, TokenSubType.String
                    )
                ),
                $"Should classify '{code}' as 'String'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }
    }
}
