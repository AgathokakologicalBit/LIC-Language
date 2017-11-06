using System;

namespace LIC.Language
{
    public struct Operator : IEquatable<Operator>
    {
        public readonly string Representation;

        public readonly uint Priority;
        public readonly bool IsRightSided;

        public Operator(string representation, uint priority, bool rightSided)
        {
            Representation = representation;
            Priority = priority;
            IsRightSided = rightSided;
        }

        #region Comparison operators
        public bool Equals(Operator other)
        {
            return
                   Representation == other.Representation
                && Priority == other.Priority
                && IsRightSided == other.IsRightSided;
        }
        #endregion
    }
}
