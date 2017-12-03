using LIC;
using LIC.Tokenization;
using NUnit.Framework;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestFixture]
    public class IdentifierTest
    {
        [Test]
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
                TokenizationTestUtils.TestEof(tokenizer);
            }
        }

        [Test]
        public void TestIdentifier_1_Invalid()
        {
            var code = "by_Я";
            var tokenizer = new Tokenizer(code, new TokenizerOptions());
            tokenizer.GetNextToken();

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
