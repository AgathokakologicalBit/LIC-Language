using LIC_Compiler;
using LIC_Compiler.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class CompilerDirectiveTest
    {
        [TestMethod]
        public void TestDirective_0_Basic()
        {
            foreach (string code in new[] { "#use", "#inline", "#run" })
            {
                var tokenizer = new Tokenizer(code, new TokenizerOptions()
                {
                    SkipWhitespace = true
                });

                Assert.IsTrue(
                    TokenizationTestUtils.Match(
                        tokenizer.GetNextToken(),
                        new Token(
                            0, code, 0, 0,
                            Tokenizer.State.Context.Global,
                            TokenType.CompilerDirective,
                            TokenSubType.CompilerDirective
                        )
                    ),
                    $"Should classify '{code}' as 'Compiler directive'"
                );
                TokenizationTestUtils.TestEOF(tokenizer);
            }
        }

        [TestMethod]
        public void TestDirective_1_SpecialSymbols()
        {
            foreach (string code in new[] { "#no_abc", "#no_optimize", "#test_it" })
            {
                var tokenizer = new Tokenizer(code, new TokenizerOptions()
                {
                    SkipWhitespace = true
                });

                Assert.IsTrue(
                    TokenizationTestUtils.Match(
                        tokenizer.GetNextToken(),
                        new Token(
                            0, code, 0, 0,
                            Tokenizer.State.Context.Global,
                            TokenType.CompilerDirective,
                            TokenSubType.CompilerDirective
                        )
                    ),
                    $"Should classify '{code}' as 'Compiler directive'"
                );
                TokenizationTestUtils.TestEOF(tokenizer);
            }
        }

        [TestMethod]
        public void TestDirective_2_InvalidCharacters()
        {
            const string code = "#тест";

            var tokenizer = new Tokenizer(code, new TokenizerOptions()
            {
                SkipWhitespace = true
            });

            Token tok = tokenizer.GetNextToken();

            Assert.IsTrue(
                tokenizer.state.IsErrorOccured(),
                "Should have an error if directive contains invalid characters"
            );
            Assert.AreEqual(
                tokenizer.state.ErrorCode,
                (uint)ErrorCodes.T_InvalidCompilerDirectiveName,
                "Should have right error type"
            );
        }

        [TestMethod]
        public void TestDirective_3_EmptyDirective()
        {
            const string code = "#";

            var tokenizer = new Tokenizer(code, new TokenizerOptions()
            {
                SkipWhitespace = true
            });

            Token tok = tokenizer.GetNextToken();

            Assert.IsTrue(
                tokenizer.state.IsErrorOccured(),
                "Should have an error if directive is not stated"
            );
            Assert.AreEqual(
                tokenizer.state.ErrorCode,
                (uint)ErrorCodes.T_CompilerDirectiveNameIsNotStated,
                "Should have right error type"
            );
        }
    }
}
