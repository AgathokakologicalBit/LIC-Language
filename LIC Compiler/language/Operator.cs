namespace LIC_Compiler.language
{
    public struct Operator
    {
        public readonly string Representation;

        public readonly uint Priority;
        public readonly bool IsRightSided;

        public Operator(string representation, uint priority, bool rightSided)
        {
            this.Representation = representation;
            this.Priority = priority;
            this.IsRightSided = rightSided;
        }

        #region Comparison operators
        public bool Equals(Operator obj)
        {
            return
                   this.Representation == obj.Representation
                && this.Priority == obj.Priority
                && this.IsRightSided == obj.IsRightSided;
        }
        #endregion
    }
}
