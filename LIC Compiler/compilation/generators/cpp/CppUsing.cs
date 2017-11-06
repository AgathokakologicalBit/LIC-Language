namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppUsing : CppCode
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Alias))
                return $"using namespace {Name.Replace(".", "::")};";

            return $"namespace {Alias.Replace(".", "::")} = {Name.Replace(".", "::")};";
        }
    }
}
