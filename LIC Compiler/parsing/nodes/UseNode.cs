namespace LIC.Parsing.Nodes
{
    public class UseNode : Node
    {
        /// <summary>
        /// Original path separated by colons(:)
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Alias path separated by colons(:)
        /// </summary>
        public string Alias { get; private set; }

        public UseNode(string path, string alias)
        {
            Path = path;
            Alias = alias;
        }

        /// <summary>
        /// Generates string representation of use node.
        /// Used for debug/logging purposes.
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Alias)
                ? $"{Path}"
                : $"{Path} as {Alias}";
        }
    }
}
