namespace LIC.Parsing.Nodes
{
    public class VariableNode : ExpressionNode
    {
        /// <summary>
        /// Variable name. Might be represented as path on access(separated by colon(:))
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Variable type
        /// </summary>
        public TypeNode Type { get; set; }

        public VariableNode(string name, TypeNode type)
        {
            Name = name;
            Type = type;
        }

        public VariableNode(string name)
            : this(name, TypeNode.AutoType)
        { }


        /// <summary>
        /// Generates string representation of variable.
        /// Used for debug/logging purposes
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Name}: {Type}";
        }
    }
}
