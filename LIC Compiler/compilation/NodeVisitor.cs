using LIC.Parsing;

namespace LIC_Compiler.compilation
{
    public abstract class NodeVisitor<TResult>
    {
        public abstract TResult Visit(Node node);
    }
}
