namespace LIC_Compiler.Tokenization.TokenParsing
{
    public interface ITokenParser
    {
        Token Parse(Tokenizer.State state);
    }
}
