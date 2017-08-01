namespace LIC.Tokenization.TokenParsing
{
    public interface ITokenParser
    {
        Token Parse(Tokenizer.State state);
    }
}
