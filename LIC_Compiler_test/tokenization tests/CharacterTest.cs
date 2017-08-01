using LIC;
using LIC.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class CharacterTest
    {
        [TestMethod]
        public void TestCharacter_0_Basic()
        {
            TestCharacter('a');
        }

        [TestMethod]
        public void TestCharacter_1_Other()
        {
            TestCharacter('#');
        }

        [TestMethod]
        public void TestCharacter_2_MisleadingCharacter()
        {
            const string code = " ";
            var tokenizer = new Tokenizer($"'{code}", new TokenizerOptions());

            tokenizer.GetNextToken();

            Assert.IsTrue(
                tokenizer.state.IsErrorOccured(),
                "Should return an error if character definition is misleading"
            );
            Assert.AreEqual(
                tokenizer.state.ErrorCode,
                (uint)ErrorCodes.T_MisleadingCharacter,
                "Should have right error type"
            );
        }

        private static void TestCharacter(char code)
        {
            var tokenizer = new Tokenizer($"'{code}", new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, code.ToString(), 0, 0,
                        Tokenizer.State.Context.Global,
                        TokenType.Character, TokenSubType.Character
                    )
                ),
                $"Should classify '{code}' as 'Character'"
            );
            TokenizationTestUtils.TestEOF(tokenizer);
        }
    }
}
