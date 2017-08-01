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
    public class StringTest
    {
        [TestMethod]
        public void TestString_0_Basic()
        {
            const string code = "!_test$";
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

        [TestMethod]
        public void TestString_1_Empty()
        {
            const string code = "";
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

        [TestMethod]
        public void TestString_2_SpecialCharacters()
        {
            const string code = "\\r\\n\\t\\\"";
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

        [TestMethod]
        public void TestString_3_NotClosed()
        {
            var code = "\"testing string";
            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            Token tok = tokenizer.GetNextToken();

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
    }
}
