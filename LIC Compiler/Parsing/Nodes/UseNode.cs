namespace LIC_Compiler.Parsing.Nodes
{
    public class UseNode : Node
    {
        public string Path { get; private set; }
        public string Alias { get; private set; }

        public UseNode(string path, string alias)
        {
            this.Path = path;
            this.Alias = alias;
        }

        public override string ToString()
        {
            return $"{Path} as {Alias}";
        }
    }
}
