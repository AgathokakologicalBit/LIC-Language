namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppUnit : CppCode
    {
        public string Value { get; set; }

        public CppUnit(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
