namespace LIC.Parsing.Nodes
{
    public class VariableNode : Node
    {
        public string Name { get; set; }
        public TypeNode Type { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Type}";
        }
    }
}
