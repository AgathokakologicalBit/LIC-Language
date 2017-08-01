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

        public Node Parent { get; set; }

        public string Name { get; set; }
        public EType Type { get; set; }

        public TypeNode ReturnType { get; set; }
        public List<VariableNode> Parameters { get; set; }
        public Node Code { get; set; }


        public FunctionNode()
        {
            Name = "";
            Type = EType.Unknown;

            Parameters = new List<VariableNode>(2);
        }


        public override string ToString()
        {
            return $"{Type.ToString().ToLower()} {Name}(\n" +
                $"  {String.Join(",\n  ", Parameters)}\n" +
                $") -> {ReturnType}";
        }
    }
}
