using LIC.Parsing.Nodes;

namespace LIC_Compiler.parsing.nodes.data_holders
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
