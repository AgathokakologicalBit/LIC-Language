using LIC;
using LIC.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class CompilerDirectiveTest
    {
        [TestMethod]
        public void TestDirective_0_Basic()
        {
            TestDirective("#use");
            TestDirective("#inline");
            TestDirective("#run");
        }

        [TestMethod]
        public void TestDirective_1_SpecialSymbols()
        {
            TestDirective("#no_abc");
            TestDirective("#no_optimize");
            TestDirective("#test_it");
        }

        [TestMethod]
        public void TestDirective_2_InvalidCharacters()
        {
            const string code = "#тест";
            var tokenizer = new Tokenizer(code, new TokenizerOptions());

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
            var tokenizer = new Tokenizer(code, new TokenizerOptions());

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

        private static void TestDirective(string directive)
        {
            var tokenizer = new Tokenizer(directive, new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, directive, 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.CompilerDirective,
                        TokenSubType.CompilerDirective
                    )
                ),
                $"Should classify '{directive}' as 'Compiler directive'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }
    }
}
