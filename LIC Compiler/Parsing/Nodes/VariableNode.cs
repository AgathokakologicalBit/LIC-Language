namespace LIC.Parsing.Nodes
{
    public class VariableNode : Node
    {
        /// <summary>
        /// Variable name. Might be represented as path on access(separated by colon(:))
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Variable type
        /// </summary>
        public TypeNode Type { get; set; }

        /// <summary>
        /// Generates string representation of variable.
        /// Used for debug/loggin purposes
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Name}: {Type}";
        }
    }
}
