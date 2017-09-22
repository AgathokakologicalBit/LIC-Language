using System;
using System.Collections.Generic;

namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppFunction : CppCode
    {
        public string Name { get; set; }
        public List<CppElement> Parameters { get; set; } = new List<CppElement>();
        public CppElement ReturnType { get; set; } = new CppElement();

        public CppBlock Code { get; set; } = new CppBlock();

        public override string ToString()
        {
            return
                $"{ReturnType} {Name}({String.Join(", ", Parameters)})\n" +
                $"{{\n" +
                $"    {Code.ToString().Replace("\n", "\n    ")}\n" +
                $"}}";
        }
    }
}
