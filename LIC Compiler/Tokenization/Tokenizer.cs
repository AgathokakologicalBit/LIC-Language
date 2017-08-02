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
            
            private Stack<Context> contextStack = new Stack<Context>(4);
            private Stack<State> _stateSaves = new Stack<State>(4);
            public TokenizerOptions Options { get; set; }
            
            /// <summary>
            /// Returns current character by inner index
            /// Returns empty symbol('\0') if no more characters are presented in code
            /// </summary>
            public char CurrentCharacter {
                get => Index < Code.Length ? Code[Index] : '\0';
            }


            /// <summary>
            /// Current character index in code string
            /// </summary>
            public int Index { get; set; };
            /// <summary>
            /// Current line number.
            /// Increments on each 'New line symbol'('\n')
            /// </summary>
            public int Line { get; set; } = 1;
            /// <summary>
            /// Index in code string pointing on line start
            /// Used to calculate position on line by current character's index
            /// </summary>
            public int LineBegin { get; set; };
            /// <summary>
            /// Returns current character's position on line starting from 1
            /// </summary>
            public int Position {
                get => Index - LineBegin + 1;
            }
            
            public State(string code)
            {
                this.Code = code;
            }

            public State(State s)
            {
                Restore(s);
            }

            
            /// <summary>
            /// Adds new context to stack
            /// </summary>
            /// <param name="context">New context</param>
            public void PushContext(Context context) => contextStack.Push(context);

            /// <summary>
            /// Returns current context without removing it from stack
            /// </summary>
            /// <returns></returns>
            public Context PeekContext() => contextStack.Peek();
            /// <summary>
            /// Returns current context and removes it from stack
            /// </summary>
            /// <returns></returns>
            public Context PopContext() => contextStack.Pop();

            /// <summary>
            /// Saves state's copy to stack
            /// </summary>
            override public void Save() => _stateSaves.Push(new State(this));
            /// <summary>
            /// Restores state's copy from stack
            /// </summary>
            override public void Restore() => Restore(_stateSaves.Pop());
            /// <summary>
            /// Removes state's copy from stack without restoring values
            /// </summary>
            override public void Drop() => _stateSaves.Pop();
            
            /// <summary>
            /// Restores state's copy from given instance
            /// </summary>
            /// <param name="state">State to copy</param>
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

            /// <summary>
            /// Returns parser that is suitable for givem context
            /// </summary>
            /// <param name="context">Target context</param>
            /// <returns>Token parser for target context</returns>
            public static Func<State, Token> GetContextParser(Context context)
            {
                switch (context)
                {
                    case Context.Global:
                    case Context.Block:
                        return GlobalParser.Parse;

                    case Context.InterpolatedString:
                        return InterpolatedStringParser.Parse;

                    default: return null;
                }
            }

            /// <summary>
            /// Returns parser that is suitable for current context
            /// </summary>
            /// <returns>Token parser for current context</returns>
            public Func<State, Token> GetCurrentParser()
                => GetContextParser(PeekContext());
        }

        /// <summary>
        /// Main tokenizer state
        /// </summary>
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
                {
                    break;
                }
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
                }

                return tok;
            }

            while (true)
            {
                bool containsErrors = state.IsErrorOccured();

                Token token = ParseNextToken();
                if (state.Options.SkipWhitespace
                    && token.Type == TokenType.Whitespace)
                {
                    continue;
                }

                if (!containsErrors)
                {
                    state.Tokens.Add(token);

                    if (token.Type != TokenType.EOF)
                    {
                        CheckErrors(token);
                    }
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
            {
                ErrorHandler.LogError(state);
            }

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
