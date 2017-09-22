using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppElement : CppCode
    {
        public List<CppCode> Parts { get; private set; } = new List<CppCode>();
        private static Regex regex = new Regex("[a-z]", RegexOptions.Compiled);

        public override string ToString()
        {
            var builder = new StringBuilder(Parts.Count * 2);
            var strings = Parts.Select(p => p.ToString()).ToArray();

            for (int i = 0; i < strings.Length; ++i)
            {
                var str = strings[i];
                var next_str = i + 1 < strings.Length ? strings[i + 1] : "";
                if (i != 0 && regex.IsMatch(str) && regex.IsMatch(next_str))
                    builder.Append(" ");
                builder.Append(str);
            }

            return builder.ToString();
        }
    }
}
