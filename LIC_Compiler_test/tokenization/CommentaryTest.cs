using LIC;
using LIC.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class CommentaryTest
    {
        [TestMethod]
        public void TestCommentary_0_Inline()
        {
            TestCommentary("// testing comments\n", TokenSubType.InlineCommentary);
        }

        [TestMethod]
        public void TestCommentary_1_Multiline()
        {
            TestCommentary("/* testing multiline comments */", TokenSubType.MultilineCommentary);
        }

        [TestMethod]
        public void TestCommentary_2_MultilineNotClosed()
        {
            string code = "/* testing multiline comments";

            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            tokenizer.GetNextToken();

            Assert.IsTrue(
                tokenizer.state.IsErrorOccured(),
                "Should have an error if commentary have no ending"
            );
            Assert.AreEqual(
                tokenizer.state.ErrorCode,
                (uint)ErrorCodes.T_UnexpectedEndOfFile,
                "Should have right error type"
            );
        }

        private static void TestCommentary(string code, TokenSubType targetType)
        {
            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, code.Trim(), 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.Commentary,
                        targetType
                    )
                ),
                $"Should classify '{code}' as '{targetType.ToString()}'"
            );
            TokenizationTestUtils.TestEof(tokenizer);
        }
    }
}
