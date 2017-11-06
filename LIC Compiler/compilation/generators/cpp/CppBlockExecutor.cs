using System;
using System.Collections.Generic;

namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppBlockExecutor : CppCode
    {
        public string Name { get; set; }
        public List<CppCode> Arguments { get; set; } = new List<CppCode>();
        public CppBlock Code { get; set; } = new CppBlock();

        public CppBlockExecutor ConnectedExecutor { get; set; }

        public CppBlockExecutor(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return
                $"{Name} ({String.Join(", ", Arguments)})\n" +
                $"{{\n" +
                $"    {Code.ToString().Replace("\n", "\n    ")}\n" +
                $"}}" + (ConnectedExecutor == null ? "" : ConnectedExecutor.ToString());
        }
    }
}
