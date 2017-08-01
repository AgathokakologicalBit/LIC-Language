using LIC_Compiler;
using LIC_Compiler.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class CommentaryTest
    {
        [TestMethod]
        public void TestCommentary_0_Inline()
        {
            string code = "// testing comments\n";

            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, code.TrimEnd(), 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.Commentary,
                        TokenSubType.InlineCommentary
                    )
                ),
                $"Should classify '{code}' as 'Inline commentary'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }

        [TestMethod]
        public void TestCommentary_1_Multiline()
        {
            string code = "/* testing multiline comments */";

            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, code, 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.Commentary,
                        TokenSubType.MultilineCommentary
                    )
                ),
                $"Should classify '{code}' as 'Multiline commentary'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }

        [TestMethod]
        public void TestCommentary_2_MultilineNotClosed()
        {
            string code = "/* testing multiline comments";

            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            Token tok = tokenizer.GetNextToken();

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
    }
}
