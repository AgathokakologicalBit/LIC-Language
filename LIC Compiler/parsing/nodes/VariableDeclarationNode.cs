using System;

namespace LIC.Parsing.Nodes
{
    public class VariableDeclarationNode : Node
    {
        /// <summary>
        /// Variable name. Might be represented as path on access(separated by colon(:))
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Variable type
        /// </summary>
        public TypeNode Type { get; set; }

        public VariableDeclarationNode(string name, TypeNode type)
        {
            this.Name = name;
            this.Type = type;
        }

        public VariableDeclarationNode(string name)
            : this(name, TypeNode.AutoType)
        {}


        /// <summary>
        /// Generates string representation of variable.
        /// Used for debug/logging purposes
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Name}: {Type}";
        }

        public override void Print(string indent)
        {
            Console.Write(this.ToString());
        }
    }
}
