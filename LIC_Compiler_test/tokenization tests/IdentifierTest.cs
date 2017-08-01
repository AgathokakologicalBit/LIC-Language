using LIC_Compiler;
using LIC_Compiler.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class IdentifierTest
    {
        [TestMethod]
        public void TestIdentifier_0_Basic()
        {
            foreach (string code in new[] { "test", "$_Test", "te5t" })
            {
                var tokenizer = new Tokenizer(code, new TokenizerOptions());

                Assert.IsTrue(
                    TokenizationTestUtils.Match(
                        tokenizer.GetNextToken(),
                        new Token(
                            0, code, 0, 0,
                            Tokenizer.State.Context.Global,
                            TokenType.Identifier, TokenSubType.Identifier
                        )
                    ),
                    $"Should classify '{code}' as 'Identifer'"
                );
                TokenizationTestUtils.TestEOF(tokenizer);
            }
        }

        [TestMethod]
        public void TestIdentifier_1_Invalid()
        {
            var code = "by_Я";
            var tokenizer = new Tokenizer(code, new TokenizerOptions());

            Token tok = tokenizer.GetNextToken();
            
            Assert.IsTrue(
                tokenizer.state.IsErrorOccured(),
                "Should return an error if identifier contains invalid characters"
            );
            Assert.AreEqual(
                tokenizer.state.ErrorCode,
                (uint)ErrorCodes.T_InvalidIdentifier,
                "Should have right error type"
            );
        }
    }
}
