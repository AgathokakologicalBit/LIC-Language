using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        /// <summary>
        /// Function's type (Static/Instance)
        /// </summary>
        public EType Type { get; set; }

        /// <summary>
        /// Return type
        /// </summary>
        public TypeNode ReturnType { get; set; }
        /// <summary>
        /// List of named parameters
        /// </summary>
        public List<VariableNode> Parameters { get; set; }
        /// <summary>
        /// Block of code or MathExpression
        /// </summary>
        public Node Code { get; set; }


        public FunctionNode()
        {
            Name = "";
            Type = EType.Unknown;

            Parameters = new List<VariableNode>(2);
        }


        /// <summary>
        /// Generates string representation of function.
        /// Used for debug/logging purposes
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Type.ToString().ToLower()} {Name}(\n" +
                $"  {String.Join(",\n  ", Parameters)}\n" +
                $") -> {ReturnType}";
        }
    }
}
