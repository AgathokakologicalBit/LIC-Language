using LIC.Parsing.ContextParsers;
using LIC.Parsing.Nodes;
using LIC.Tokenization;
using System.Collections.Generic;
using System;

namespace LIC.Parsing
{
    public class Parser
    {
        public class State : LIC.State
        {
            private int _index;
            private Stack<State> _stateSaves;


            public State(Token[] tokens, string code)
            {
                this.Code = code;

                this.Tokens = new List<Token>(tokens);
                this._index = -1;

                this.ErrorCode = 0;
                this.ErrorMessage = "";

                this._stateSaves = new Stack<State>(2);
            }

            public State(State state)
            {
                this.Restore(state);
            }


            /// <summary>
            /// Gets next available token or null
            /// </summary>
            /// <returns>Next token</returns>
            public Token GetNextToken()
            {
                if (_index >= Tokens.Count)
                    return null;

                _index += 1;
                return GetToken();
            }

            /// <summary>
            /// Gets current token or null
            /// Then moves to next token
            /// </summary>
            /// <returns>Current token</returns>
            public Token GetTokenAndMove()
            {
                Token tok = GetToken();
                if (tok == null)
                    return null;

                _index += 1;
                return tok;
            }

            /// <summary>
            /// Gets current token or null
            /// </summary>
            /// <returns>Current token</returns>
            public Token GetToken()
            {
                if (_index >= Tokens.Count)
                    return null;
                if (_index < 0)
                    _index = 0;

                return Tokens[_index];
            }

            /// <summary>
            /// Gets next valuable token or null
            /// valuable - not empty/commentary
            /// </summary>
            /// <returns>Next valuable token</returns>
            public Token GetNextNEToken()
            {
                Token t;

                do
                {
                    t = GetNextToken();
                } while (t?.Type == TokenType.Commentary);

                return t;
            }

            /// <summary>
            /// Gets current token or null
            /// Then moves to the next valuable token
            /// valuable - not empty/commentary
            /// </summary>
            /// <returns>Current token</returns>
            public Token GetTokenAndMoveNE()
            {
                Token tok = GetToken();
                GetNextNEToken();
                return tok;
            }

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
            public override void Drop() => _stateSaves.Pop();

            /// <summary>
            /// Restores state's copy from given instance
            /// </summary>
            /// <param name="state">Target state</param>
            public void Restore(State state)
            {
                _index = state._index;
                Tokens = state.Tokens;
                _stateSaves = state._stateSaves;
            }
        }

        private State state;

        /// <summary>
        /// Performs parsing of whole module (file)
        /// </summary>
        /// <param name="tokens">Tokens to parse</param>
        /// <param name="code">Code (for errors reporting)</param>
        /// <returns>Parsed AST</returns>
        public CoreNode Parse(Token[] tokens, string code)
        {
            state = new State(tokens, code);
            var result = ModuleParser.Parse(state);

            if (state.IsErrorOccured())
                ReportError(state);

            return result;
        }

        private void ReportError(State state)
        {
            Console.Error.WriteLine($"Error #LC{state.ErrorCode.ToString("D3")}:");
            Console.Error.WriteLine(state.ErrorMessage);
            Console.Error.WriteLine();

            state.Restore();
            var lastToken = state.GetToken();

            Console.Error.WriteLine($"Line: {lastToken.Line}");
            Console.Error.WriteLine($"Position: {lastToken.Position}");
            Console.Error.WriteLine();
        }
    }
}
