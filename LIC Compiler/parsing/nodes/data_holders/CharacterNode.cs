namespace LIC.Parsing.Nodes
{
    public class CharacterNode : ExpressionNode
    {
        public string CharacterValue { get; private set; }


        public CharacterNode(string value)
        {
            this.CharacterValue = value;

            this.Value = this;
        }
    }
}
