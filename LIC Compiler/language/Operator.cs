using System;

namespace LIC_Compiler.language
{
    public struct Operator : IEquatable<Operator>
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
        public bool Equals(Operator other)
        {
            return
                   this.Representation == other.Representation
                && this.Priority == other.Priority
                && this.IsRightSided == other.IsRightSided;
        }
        #endregion
    }
}
