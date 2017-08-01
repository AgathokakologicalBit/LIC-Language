using System;
using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class TypeNode : Node
    {
        public string TypePath { get; set; }

        public bool IsConstant { get; set; }
        public bool IsDynamic { get; set; }

        public bool IsReference { get; set; }
        public bool IsValueType { get; set; }

        public bool IsArrayType { get; set; }

        public TypeNode ReferenceType { get; set; }

        public override string ToString()
        {
            List<string> modifiers = new List<string>(5);
            if (IsConstant) modifiers.Add("const");
            if (IsDynamic) modifiers.Add("dynamic");
            if (IsReference) modifiers.Add("ref");
            if (IsValueType) modifiers.Add("val");
            if (IsArrayType) modifiers.Add("array");
            
            string modStr = String.Join(" ", modifiers);
            if (modStr != "") modStr += " ";

            if (ReferenceType == null)
                return $"{modStr}{TypePath}";

            return $"{modStr}({ReferenceType})";
        }
    }
}
