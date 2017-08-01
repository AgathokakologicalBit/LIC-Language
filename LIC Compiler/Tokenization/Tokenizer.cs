using LIC.Parsing;
using LIC.Tokenization.TokenParsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LIC.Tokenization
{
    public class Tokenizer
    {
        public class State : LIC.State
        {
            public enum Context
            {
                Global,
                Block,

                InterpolatedString,
                RegularExpression
            }
            
            private Stack<Context> contextStack;

            private Stack<State> _stateSaves;

            public TokenizerOptions Options { get; set; }
            
            public char CurrentCharacter {
                get => Index < Code.Length ? Code[Index] : '\0';
            }


            public int Index { get; set; }
            public int Line { get; set; }
            public int LineBegin { get; set; }
            public int Position {
                get => Index - LineBegin + 1;
            }

            public uint Depth { get => (uint)contextStack.Count; }
            
            public State(string code)
            {
                this.Code = code;
                this.Index = 0;

                this.Line = 1;
                this.LineBegin = 0;

                this.ErrorCode = 0;
                this.ErrorMessage = "";

                this.contextStack = new Stack<Context>(8);
                this._stateSaves = new Stack<State>(8);
            }

            public State(State state)
            {
                Restore(state);
            }

            
            public void PushContext(Context context) => contextStack.Push(context);

            public Context PeekContext() => contextStack.Peek();
            public Context PopContext() => contextStack.Pop();


            override public void Save() => _stateSaves.Push(new State(this));
            override public void Restore() => Restore(_stateSaves.Pop());
            override public void Drop() => _stateSaves.Pop();
            
            public void Restore(State state)
            {
                this.Code = state.Code;

                this.Index = state.Index;
                this.Line = state.Line;
                this.LineBegin = state.LineBegin;

                this.ErrorCode = state.ErrorCode;
                this.ErrorMessage = state.ErrorMessage;

                this.contextStack = state.contextStack;
                this._stateSaves = state._stateSaves;
            }

            public Func<State, Token> GetContextParser(Context context)
            {
                switch(context)
                {
                    case Context.Global:
                    case Context.Block:
                        return GlobalParser.Parse;
                    
                    case Context.InterpolatedString:
                        return InterpolatedStringParser.Parse;
                }

                return null;
            }

            public Func<State, Token> GetCurrentParser()
                => GetContextParser(PeekContext());
        }

        public State state;

        public Tokenizer(string code, TokenizerOptions options)
        {
            state = new State(code)
            {
                Options = options
            };
            state.PushContext(State.Context.Global);

            state.Tokens = new List<Token>(state.Code.Length / 8 + 1);
        }

        /// <summary>
        /// Performs tokenization of whole code
        /// </summary>
        /// <returns>Array of tokens</returns>
        public Token[] Tokenize()
        {
            while (true)
            {
                Token token = GetNextToken();
                
                if (token.Type == TokenType.EOF)
                    break;
            }

            return state.Tokens.ToArray();
        }

        /// <summary>
        /// Tokenizes only part of code that contains token.
        /// Or throws TokenizationException if token can not be parsed
        /// </summary>
        /// <returns>Next token of code</returns>
        public Token GetNextToken()
        {
            if (state.Index >= state.Code.Length)
            {
                var tok = new Token(
                    state.Index, "", state.Line, state.Position,
                    state.PeekContext(),
                    TokenType.EOF,
                    TokenSubType.EndOfFile
                );

                if (state.Tokens.Count == 0
                    || state.Tokens.Last().Type != TokenType.EOF)
                {
                    state.Tokens.Add(tok);
                    // state.Index += 1;
                }

                return tok;
            }

            while (true)
            {
                bool containsErrors = state.IsErrorOccured();

                Token token = ParseNextToken();
                if (state.Options.SkipWhitespace
                    && token.Type == TokenType.Whitespace)
                    continue;

                if (!containsErrors)
                {
                    state.Tokens.Add(token);

                    if (token.Type != TokenType.EOF)
                        CheckErrors(token);
                }

                return token;
            }
        }

        private Token ParseNextToken()
        {
            Token token = null;
            token = state.GetCurrentParser()?.Invoke(state);

            if (token == null)
            {
                token = new Token(
                    state.Index, "", state.Line, state.Position,
                    state.PeekContext(),
                    TokenType.EOF,
                    TokenSubType.EndOfFile
                );
            }
            
            return token;
        }

        private bool CheckErrors(Token lastToken)
        {
            if (state.IsErrorOccured())
                 ErrorHandler.LogError(state);

            if (lastToken.Type == TokenType.EOF
                && state.PeekContext() != State.Context.Global)
            {
                state.ErrorCode = (uint)ErrorCodes.T_UnexpectedEndOfFile;
                state.ErrorMessage = "Unexpected end of file\n" +
                    $"Context '{state.PeekContext().ToString()}' wasn't closed";

                ErrorHandler.LogError(state);
                return true;
            }

            return state.IsErrorOccured();
        }
    }
}
