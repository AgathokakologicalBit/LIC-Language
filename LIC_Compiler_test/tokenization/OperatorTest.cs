using LIC.Tokenization;
using NUnit.Framework;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestFixture]
    public class OperatorTest
    {
        [Test]
        public void TestOperator_0_Plus()
            => TestMathOp('+', TokenSubType.Plus);
        [Test]
        public void TestOperator_1_Dash()
            => TestMathOp('-', TokenSubType.Dash);
        [Test]
        public void TestOperator_2_Star()
            => TestMathOp('*', TokenSubType.Star);
        [Test]
        public void TestOperator_3_Slash()
            => TestMathOp('/', TokenSubType.Slash);


        [Test]
        public void TestOperator_4_Backslash()
            => TestSpecOp('\\', TokenSubType.Backslash);
        [Test]
        public void TestOperator_5_Dot()
            => TestSpecOp('.', TokenSubType.Dot);
        [Test]
        public void TestOperator_6_Caret()
            => TestMathOp('^', TokenSubType.Caret);


        [Test]
        public void TestOperator_7_Braces()
        {
            TestSpecOp('(', TokenSubType.BraceRoundLeft);
            TestSpecOp(')', TokenSubType.BraceRoundRight);

            TestMathOp('<', TokenSubType.BraceTriangularLeft);
            TestMathOp('>', TokenSubType.BraceTriangularRight);

            TestSpecOp('[', TokenSubType.BraceSquareLeft);
            TestSpecOp(']', TokenSubType.BraceSquareRight);

            TestSpecOp('{', TokenSubType.BraceCurlyLeft);
            TestSpecOp('}', TokenSubType.BraceCurlyRight);
        }


        [Test]
        public void TestOperator_8_Equal()
            => TestMathOp('=', TokenSubType.Equal);
        [Test]
        public void TestOperator_9_Ampersand()
            => TestMathOp('&', TokenSubType.Ampersand);
        [Test]
        public void TestOperator_A_VerticalBar()
            => TestMathOp('|', TokenSubType.VerticalBar);


        [Test]
        public void TestOperator_B_QuestionMark()
            => TestSpecOp('?', TokenSubType.QuestionMark);
        [Test]
        public void TestOperator_C_ExclamationMark()
            => TestSpecOp('!', TokenSubType.ExclamationMark);


        [Test]
        public void TestOperator_D_Colon()
            => TestSpecOp(':', TokenSubType.Colon);
        [Test]
        public void TestOperator_E_Semicolon()
            => TestSpecOp(';', TokenSubType.SemiColon);


        [Test]
        public void TestOperator_F_Comma()
            => TestSpecOp(',', TokenSubType.Comma);


        private void TestMathOp(char op, TokenSubType st)
            => TestOperator(op.ToString(), st, TokenType.MathOperator);
        private void TestSpecOp(char op, TokenSubType st)
            => TestOperator(op.ToString(), st, TokenType.SpecialOperator);

        private void TestOperator(string op, TokenSubType st, TokenType t)
        {
            var tokenizer = new Tokenizer(op, new TokenizerOptions());

            Assert.IsTrue(
                TokenizationTestUtils.Match(
                    tokenizer.GetNextToken(),
                    new Token(
                        0, op, 0, 0,
                        Tokenizer.State.Context.Global,
                        t, st
                    )
                ),
                $"Should classify '{op}' as '{st}'"
            );
            TokenizationTestUtils.TestEof(tokenizer);
        }
    }
}
