using System;

namespace LIC.Tokenization
{
    public class Token
    {
        public Tokenizer.State.Context Context { get; set; }

        public int Index { get; set; }
        public int Line { get; set; }
        public int Position { get; set; }

        public string Value { get; set; }
        public int Length { get; set; }

        public TokenType Type { get; set; }
        public TokenSubType SubType { get; set; }

        public Token
        (
            string value,
            TokenType type, TokenSubType subType
        )
            : this(
                0, value, 0, 0,
                Tokenizer.State.Context.Global,
                type, subType
            )
        { }

        public Token
        (
            int index,
            string value,

            int line, int position,

            Tokenizer.State.Context context,
            TokenType type, TokenSubType subType
        )
        {
            this.Context = context;

            this.Index = index;
            this.Line = line;
            this.Position = position;

            this.Value = value;
            this.Length = value.Length;

            this.Type = type;
            this.SubType = subType;
        }

        public override string ToString()
        {
            return  $"({Line.ToString().PadLeft(3, '0')}:" +
                    $"{Position.ToString().PadLeft(3, '0')}) " +
                    $"{SubType.ToString().PadRight(20, ' ')} " +
                    $"({Length.ToString().PadLeft(2, ' ')}) {Value}";
        }

        public bool Is(TokenType t, string v) => Type == t && Value == v;
        public bool Is(TokenSubType st, string v) => SubType == st && Value == v;
    }
}
