using LIC_Compiler.Tokenization;
using System.Collections.Generic;

namespace LIC_Compiler
{
    public abstract class State
    {
        public string Code { get; set; }
        public List<Token> Tokens { get; set; }

        public uint ErrorCode { get; set; }
        public string ErrorMessage { get; set; }


        public bool IsErrorOccured() => ErrorCode > 0;

        public abstract void Save();
        public abstract void Restore();
        public abstract void Drop();
    }
}
