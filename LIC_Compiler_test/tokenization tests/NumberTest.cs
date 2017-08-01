using Microsoft.VisualStudio.TestTools.UnitTesting;
using LIC.Tokenization;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class NumberTest
    {
        [TestMethod]
        public void TestNumber_0_Integer()
        {
            var code = "1234567890";
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
                        TokenType.Number, TokenSubType.Integer
                    )
                ),
                "Should tokenize number '1234567890' and classify it as 'Integer'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }

        [TestMethod]
        public void TestNumber_1_Decimal()
        {
            var code = "12345.67890";
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
                        TokenType.Number, TokenSubType.Decimal
                    )
                ),
                "Should tokenize number '12345.67890' and classify it as 'Decimal'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }
    }
}
