using LIC_Compiler.Tokenization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIC_Compiler_test.TokenizationTests
{
    [TestClass]
    public class OperatorTest
    {
        [TestMethod]
        public void TestOperator_0_Plus()
            => TestMathOp('+', TokenSubType.Plus);
        [TestMethod]
        public void TestOperator_1_Dash()
            => TestMathOp('-', TokenSubType.Dash);
        [TestMethod]
        public void TestOperator_2_Star()
            => TestMathOp('*', TokenSubType.Star);
        [TestMethod]
        public void TestOperator_3_Slash()
            => TestMathOp('/', TokenSubType.Slash);


        [TestMethod]
        public void TestOperator_4_Backslash()
            => TestSpecOp('\\', TokenSubType.Backslash);
        [TestMethod]
        public void TestOperator_5_Dot()
            => TestSpecOp('.', TokenSubType.Dot);
        [TestMethod]
        public void TestOperator_6_Caret()
            => TestMathOp('^', TokenSubType.Caret);


        [TestMethod]
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

        
        [TestMethod]
        public void TestOperator_8_Equal()
            => TestMathOp('=', TokenSubType.Equal);
        [TestMethod]
        public void TestOperator_9_Ampersand()
            => TestMathOp('&', TokenSubType.Ampersand);
        [TestMethod]
        public void TestOperator_A_VerticalBar()
            => TestMathOp('|', TokenSubType.VerticalBar);


        [TestMethod]
        public void TestOperator_B_QuestionMark()
            => TestSpecOp('?', TokenSubType.QuestionMark);
        [TestMethod]
        public void TestOperator_C_ExclamationMark()
            => TestSpecOp('!', TokenSubType.ExclamationMark);


        [TestMethod]
        public void TestOperator_D_Colon()
            => TestSpecOp(':', TokenSubType.Colon);
        [TestMethod]
        public void TestOperator_E_Semicolon()
            => TestSpecOp(';', TokenSubType.SemiColon);


        [TestMethod]
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
            TokenizationTestUtils.TestEOF(tokenizer);
        }
    }
}
