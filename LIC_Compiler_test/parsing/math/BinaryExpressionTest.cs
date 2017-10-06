using LIC.Parsing;
using LIC.Parsing.ContextParsers;
using LIC.Parsing.Nodes;
using LIC.Tokenization;
using LIC_Compiler.parsing.nodes;
using LIC_Compiler.parsing.nodes.data_holders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIC_Compiler_test.parsing.math
{
    [TestClass]
    public class BinaryExpressionTest
    {
        [TestMethod]
        public void TestBinaryExpression_0_Simple()
        {
            TestExpression(2, "+", 1);
            TestExpression(2, "-", 1);
            TestExpression(2, "*", 1);
            TestExpression(2, "/", 1);
            TestExpression(2, "^", 1);
        }

        [TestMethod]
        public void TestBinaryExpression_1_Comparison()
        {
            TestExpression(2, "==", 1);
            TestExpression(2, "!=", 1);
            TestExpression(2,  "<", 1);
            TestExpression(2, "<=", 1);
            TestExpression(2,  ">", 1);
            TestExpression(2, ">=", 1);
        }

        [TestMethod]
        public void TestBinaryExpression_2_Assignment()
        {
            TestExpression("a", "=", "b");
            TestExpression("a", "+=", "b");
            TestExpression("a", "-=", "b");
            TestExpression("a", "*=", "b");
            TestExpression("a", "/=", "b");
        }

        [TestMethod]
        public void TestBinaryExpression_3_Special()
        {
            TestExpression("a", ".", "b");
        }

        private static void TestTypes(BinaryOperatorNode ast, Type t)
        {
            Assert.IsNotNull(ast);
            Assert.IsInstanceOfType(ast, typeof(BinaryOperatorNode));

            var bin = ast as BinaryOperatorNode;
            Assert.IsInstanceOfType(bin.LeftOperand, t);
            Assert.IsInstanceOfType(bin.RightOperand, t);
        }

        private static void TestExpression(long a, string op, long b)
        {
            var ast = Parse($"{a} {op} {b}") as BinaryOperatorNode;
            TestTypes(ast, typeof(NumberNode));

            var left = ast.LeftOperand as NumberNode;
            var right = ast.RightOperand as NumberNode;

            Assert.AreEqual(a.ToString(), left.NumericValue);
            Assert.AreEqual(b.ToString(), right.NumericValue);
            Assert.AreEqual(op, ast.Operation.Representation);
        }

        private static void TestExpression(string a, string op, string b)
        {
            var ast = Parse($"{a} {op} {b}") as BinaryOperatorNode;
            TestTypes(ast, typeof(VariableNode));

            var left = ast.LeftOperand as VariableNode;
            var right = ast.RightOperand as VariableNode;

            Assert.AreEqual(a.ToString(), left.Name);
            Assert.AreEqual(b.ToString(), right.Name);
            Assert.AreEqual(op, ast.Operation.Representation);
        }

        private static ExpressionNode Parse(string data)
        {
            var tokenizer = new Tokenizer(data, new TokenizerOptions());
            var state = new Parser.State(tokenizer.Tokenize());
            return MathExpressionParser.Parse(state);
        }
    }
}
