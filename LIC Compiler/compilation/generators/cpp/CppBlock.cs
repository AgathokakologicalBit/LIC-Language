using System;
using System.Collections.Generic;

namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppBlock : CppCode
    {
        public List<CppCode> Elements { get; set; } = new List<CppCode>();

        public override string ToString()
        {
            return String.Join(";\n", Elements) + ";";
        }
    }
}
