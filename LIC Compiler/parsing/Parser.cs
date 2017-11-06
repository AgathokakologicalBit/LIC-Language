using LIC.Parsing.ContextParsers;
using LIC.Parsing.Nodes;
using LIC.Tokenization;
using System.Collections.Generic;
using LIC_Compiler.parsing.nodes;
using System;

namespace LIC.Parsing
{
    public static class Parser
    {
        public class State : LIC.State
        {
            private int _index = -1;
            private Stack<State> _stateSaves = new Stack<State>(2);
            private List<FunctionCallNode> _attributes = new List<FunctionCallNode>();

            public int Index { get => _index; set => _index = value; }


            public State(Token[] tokens)
            {
                this.Tokens = new List<Token>(tokens);
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
                if (_index >= Tokens.Count) { return null; }

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
                if (tok == null) { return null; }

                _index += 1;
                return tok;
            }

            /// <summary>
            /// Gets current token or null
            /// </summary>
            /// <returns>Current token</returns>
            public Token GetToken()
            {
                if (_index >= Tokens.Count) { return null; }
                if (_index < 0) { _index = 0; }

                return Tokens[_index];
            }

            /// <summary>
            /// Gets next valuable token or null
            /// valuable - not empty/commentary
            /// </summary>
            /// <returns>Next valuable token</returns>
            public Token GetNextNeToken()
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
            public Token GetTokenAndMoveNe()
            {
                Token tok = GetToken();
                GetNextNeToken();
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

            public int GetIndex()
            {
                return _index;
            }

            internal void PushdAttribute(FunctionCallNode call)
            {
                this._attributes.Add(call);
            }

            internal void ClearAttributes()
            {
                this._attributes.Clear();
            }

            internal List<FunctionCallNode> GetAttributes()
            {
                return this._attributes;
            }
        }

        /// <summary>
        /// Performs parsing of whole module (file)
        /// </summary>
        /// <param name="tokens">Tokens to parse</param>
        /// <returns>Parsed AST</returns>
        public static CoreNode Parse(Token[] tokens)
        {
            State state = new State(tokens);
            var result = ModuleParser.Parse(state);

            if (state.IsErrorOccured())
            {
                ReportError(state);
                return null;
            }

            return result;
        }

        private static void ReportError(State state)
        {
            ErrorHandler.LogErrorInfo(state);
            ErrorHandler.LogErrorPosition(state.GetToken());
            ErrorHandler.LogErrorTokensPart(state, state.GetIndex(), 3, 2);
        }
    }
}
