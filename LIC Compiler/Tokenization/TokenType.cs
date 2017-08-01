namespace LIC_Compiler.Tokenization
{
    public enum TokenType
    {
        Unknown,

        Commentary,

        Identifier,
        CompilerDirective,

        Whitespace,

        Number,
        String,
        Character,

        MathOperator,
        SpecialOperator,

        InterpolatedString,

        EOF
    }

    public enum TokenSubType
    {
        Unknown,

        InlineCommentary, MultilineCommentary,

        Identifier, Keyword,
        CompilerDirective,

        Space, NewLine,

        Integer, Decimal,

        String,
        Character,

        AtSign,

        BraceRoundLeft, BraceRoundRight,
        BraceSquareLeft, BraceSquareRight,
        BraceTriangularLeft, BraceTriangularRight,
        BraceCurlyLeft, BraceCurlyRight,

        Plus, Dash,
        Star, Slash, Backslash,
        Dot, Comma,

        VerticalBar, Ampersand, Caret,

        Equal, ExclamationMark, QuestionMark,
        Colon, SemiColon,

        InterpolatedStringBegin, InterpolatedStringEnd,

        EndOfFile
    }
}
