using LIC_Compiler.Parsing.ContextParsers;
using LIC_Compiler.Parsing.Nodes;
using LIC_Compiler.Tokenization;
using System.Collections.Generic;
using System;

namespace LIC_Compiler.Parsing
{
    public class Parser
    {
        public class State : LIC_Compiler.State
        {
            private Token[] _tokens;
            private int _index;

            private Stack<State> _stateSaves;


            public State(Token[] tokens, string code)
            {
                this.Code = code;

                this._tokens = tokens;
                this._index = -1;

                this.ErrorCode = 0;
                this.ErrorMessage = "";

                this._stateSaves = new Stack<State>(2);
            }

            public State(State state)
            {
                this.Restore(state);
            }


            public Token GetNextToken()
            {
                if (_index >= _tokens.Length)
                    return null;

                _index += 1;
                return GetToken();
            }

            public Token GetTokenAndMove()
            {
                Token tok = GetToken();
                if (tok == null)
                    return null;

                _index += 1;
                return tok;
            }

            public Token GetToken()
            {
                if (_index >= _tokens.Length)
                    return null;
                if (_index < 0)
                    _index = 0;

                return _tokens[_index];
            }

            public Token GetNextNEToken()
            {
                Token t;

                do
                {
                    t = GetNextToken();
                } while (t.Type == TokenType.Commentary);

                return t;
            }

            public Token GetTokenAndMoveNE()
            {
                Token tok = GetToken();
                GetNextNEToken();
                return tok;
            }


            override public void Save() => _stateSaves.Push(new State(this));
            override public void Restore() => Restore(_stateSaves.Pop());
            public override void Drop() => _stateSaves.Pop();


            public void Restore(State state)
            {
                _index = state._index;
                _tokens = state._tokens;
                _stateSaves = state._stateSaves;
            }
        }

        private State state;

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
