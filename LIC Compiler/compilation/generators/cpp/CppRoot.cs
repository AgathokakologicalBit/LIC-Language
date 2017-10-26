using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppRoot : CppCode
    {
        public List<CppUsing> Usings { get; private set; }
        public List<CppFunction> Functions { get; private set; }

        public CppRoot()
        {
            this.Usings = new List<CppUsing>();
            this.Functions = new List<CppFunction>();
        }

        public override string ToString()
        {
            // Init string builder to hold up to 32MB by default
            var builder = new StringBuilder(32 * 1024 * 1024);
            builder
                .AppendLine("/*")
                .AppendLine(" *   Source code was automatically generated!")
                .AppendLine(" *   Do NOT make any changes to this file")
                .AppendLine(" *     because they might be overwritten after an update.")
                .AppendLine(" * ")
                .AppendLine(" *   Compiler: lcc v" + Assembly.GetExecutingAssembly().GetName().Version)
                .AppendLine(" */")
                .AppendLine()
                .AppendLine("#include <cstddef>")
                .AppendLine("#include \"lic/core.cpp\"")
                .AppendLine("#include \"lic/io.cpp\"\n")
                .AppendLine()
                .AppendLine("using namespace lic;");


            foreach (var u in Usings)
                builder.AppendLine(u.ToString());

            builder.AppendLine();

            foreach (var f in Functions)
                builder.AppendLine(f.ToString()).AppendLine();

            return builder.ToString();
        }
    }
}
