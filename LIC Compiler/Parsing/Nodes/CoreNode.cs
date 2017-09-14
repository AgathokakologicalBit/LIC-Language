using System;
using System.Collections.Generic;

namespace LIC.Parsing.Nodes
{
    public class CoreNode : Node
    {
        public List<UseNode> UsesNodes { get; private set; } = new List<UseNode>();
        public List<ClassNode> ClassNodes { get; private set; } = new List<ClassNode>();
        public List<FunctionNode> FunctionNodes { get; private set; } = new List<FunctionNode>();

        public override void Print(string indent)
        {
            throw new NotImplementedException();
        }

        internal void Print()
        {
            foreach (var use in UsesNodes)
                use.Print("");

            foreach (var clss in ClassNodes)
                clss.Print("");

            foreach (var func in FunctionNodes)
                func.Print("");
        }
    }
}
