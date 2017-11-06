using System;
using System.Collections.Generic;
using System.Globalization;

namespace LIC.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public enum EType
        {
            Unknown,

            Static,
            Instance
        }

        /// <summary>
        /// Parent Class/Function
        /// </summary>
        public Node Parent { get; set; }

        /// <summary>
        /// Function's identifier
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Function's type (Static/Instance)
        /// </summary>
        public EType Type { get; set; } = EType.Unknown;

        /// <summary>
        /// Return type
        /// </summary>
        public TypeNode ReturnType { get; set; }
        /// <summary>
        /// List of named parameters
        /// </summary>
        public List<VariableDeclarationNode> Parameters { get; set; } = new List<VariableDeclarationNode>(2);
        /// <summary>
        /// Block of code or MathExpression
        /// </summary>
        public Node Code { get; set; }

        /// <summary>
        /// List of attribute classes (functions at this moment)
        /// </summary>
        public List<FunctionCallNode> Attributes { get; set; } = new List<FunctionCallNode>();


        /// <summary>
        /// Generates string representation of function.
        /// Used for debug/logging purposes
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Type.ToString().ToLower(CultureInfo.InvariantCulture)} {Name}{{\n" +
                $"  {String.Join(",\n  ", Parameters)}\n" +
                $"}} -> {ReturnType}";
        }
    }
}
