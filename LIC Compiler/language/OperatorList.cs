using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LIC.Language
{
    public static class OperatorList
    {
        #region Operators
        public static readonly Operator Unknown = new Operator();

        #region Main operators (1000..750)
        public static readonly Operator MemberAccess
            = new Operator(
                representation: ".",
                priority: 1000,
                rightSided: false
            );

        public static readonly Operator Assignment
            = new Operator(
                representation: "=",
                priority: 750,
                rightSided: true
            );
        #endregion

        #region Assignment with math action operators (500..500)
        public static readonly Operator AssignmentAddition
            = new Operator(
                representation: "+=",
                priority: 500,
                rightSided: true
            );
        public static readonly Operator AssignmentSubtraction
            = new Operator(
                representation: "-=",
                priority: 500,
                rightSided: true
            );
        public static readonly Operator AssignmentMultiplication
            = new Operator(
                representation: "*=",
                priority: 500,
                rightSided: true
            );
        public static readonly Operator AssignmentDivision
            = new Operator(
                representation: "/=",
                priority: 500,
                rightSided: true
            );
        public static readonly Operator AssignmentPower
            = new Operator(
                representation: "^=",
                priority: 500,
                rightSided: true
            );
        public static readonly Operator AssignmentModulo
            = new Operator(
                representation: "%=",
                priority: 500,
                rightSided: true
            );
        #endregion

        #region Assignment with bitwise action operators (500..500)
        public static readonly Operator AssignmentAnd
            = new Operator(
                representation: "&=",
                priority: 500,
                rightSided: true
            );
        public static readonly Operator AssignmentOr
            = new Operator(
                representation: "|=",
                priority: 500,
                rightSided: true
            );
        #endregion

        #region Math operators (250..100)
        public static readonly Operator Power
            = new Operator(
                representation: "^",
                priority: 250,
                rightSided: false
            );
        public static readonly Operator Modulo
            = new Operator(
                representation: "%",
                priority: 200,
                rightSided: false
            );
        public static readonly Operator Multiplication
            = new Operator(
                representation: "*",
                priority: 150,
                rightSided: false
            );
        public static readonly Operator Division
            = new Operator(
                representation: "/",
                priority: 150,
                rightSided: false
            );
        public static readonly Operator Addition
            = new Operator(
                representation: "+",
                priority: 100,
                rightSided: false
            );
        public static readonly Operator Subtraction
            = new Operator(
                representation: "-",
                priority: 100,
                rightSided: false
            );
        #endregion

        #region Comparison operators (50..40)
        public static readonly Operator GreaterOrEqual
            = new Operator(
                representation: ">=",
                priority: 50,
                rightSided: false
            );
        public static readonly Operator LessOrEqual
            = new Operator(
                representation: "<=",
                priority: 50,
                rightSided: false
            );
        public static readonly Operator Greater
            = new Operator(
                representation: ">",
                priority: 50,
                rightSided: false
            );
        public static readonly Operator Less
            = new Operator(
                representation: "<",
                priority: 50,
                rightSided: false
            );

        public static readonly Operator Equal
            = new Operator(
                representation: "==",
                priority: 40,
                rightSided: false
            );
        public static readonly Operator NotEqual
            = new Operator(
                representation: "!=",
                priority: 40,
                rightSided: false
            );
        public static readonly Operator EqualReference
            = new Operator(
                representation: "===",
                priority: 40,
                rightSided: false
            );
        public static readonly Operator NotEqualReference
            = new Operator(
                representation: "!==",
                priority: 40,
                rightSided: false
            );
        #endregion

        #region Bitwise operators (25..20)
        public static readonly Operator BitwiseAnd
            = new Operator(
                representation: "&",
                priority: 25,
                rightSided: false
            );
        public static readonly Operator BitwiseOr
            = new Operator(
                representation: "|",
                priority: 20,
                rightSided: false
            );
        #endregion

        #region Logical operators (10..5)
        public static readonly Operator LogicalAnd
            = new Operator(
                representation: "&&",
                priority: 10,
                rightSided: false
            );
        public static readonly Operator LogicalOr
            = new Operator(
                representation: "||",
                priority: 5,
                rightSided: false
            );
        #endregion
        #endregion

        #region Operators group lists
        public static readonly ReadOnlyCollection<Operator> MainOperators
            = new List<Operator>
            {
                MemberAccess,
                Assignment
            }.AsReadOnly();
        public static readonly ReadOnlyCollection<Operator> AssignmentWithMathActionOperators
            = new List<Operator>
            {
                AssignmentPower,
                AssignmentModulo,
                AssignmentMultiplication, AssignmentDivision,
                AssignmentAddition, AssignmentSubtraction
            }.AsReadOnly();
        public static readonly ReadOnlyCollection<Operator> AssignmentWithBitwiseActionOperators
            = new List<Operator>
            {
                AssignmentAnd,
                AssignmentOr
            }.AsReadOnly();
        public static readonly ReadOnlyCollection<Operator> MathOperators
            = new List<Operator>
            {
                Power,
                Modulo,
                Multiplication, Division,
                Addition, Subtraction
            }.AsReadOnly();
        public static readonly ReadOnlyCollection<Operator> BitwiseOperators
            = new List<Operator>
            {
                BitwiseAnd, BitwiseOr
            }.AsReadOnly();
        public static readonly ReadOnlyCollection<Operator> ComparisonOperators
            = new List<Operator>
            {
                GreaterOrEqual, LessOrEqual,
                Greater, Less,
                Equal, NotEqual,
                EqualReference, NotEqualReference
            }.AsReadOnly();
        public static readonly ReadOnlyCollection<Operator> LogicalOperators
            = new List<Operator>
            {
                LogicalAnd,
                LogicalOr
            }.AsReadOnly();
        #endregion

        #region All operators list
        public static readonly ReadOnlyCollection<Operator> Operators
            = new[]
            {
                MainOperators,

                AssignmentWithMathActionOperators,
                AssignmentWithBitwiseActionOperators,

                MathOperators,
                BitwiseOperators,

                ComparisonOperators,
                LogicalOperators
            }.SelectMany(operatorsList => operatorsList).ToList().AsReadOnly();
        #endregion
    }
}
