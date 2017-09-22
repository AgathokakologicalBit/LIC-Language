using LIC.Parsing;
using LIC.Parsing.Nodes;
using LIC_Compiler.parsing.nodes;
using LIC_Compiler.parsing.nodes.data_holders;
using System;

namespace LIC_Compiler.compilation.generators.cpp
{
    public class CppGenerationVisitor : NodeVisitor<CppCode>
    {
        bool _rcall = false;
        
        public override CppCode Visit(Node node)
        {
            if (_rcall)
            {
                Console.WriteLine("Unable to visit the following node:");
                Console.WriteLine(node.GetType().FullName);

                throw new NotImplementedException();
            }

            _rcall = true;
            var res = Visit((dynamic)node);

            return res;
        }

        public CppCode Visit(CoreNode node)
        {
            var root = new CppRoot();

            foreach (var useNode in node.UsesNodes)
                root.Usings.Add((CppUsing)Visit(useNode));

            foreach (var funcNode in node.FunctionNodes)
                root.Functions.Add((CppFunction)Visit(funcNode));

            return root;
        }

        public CppCode Visit(UseNode node)
        {
            _rcall = false;

            return new CppUsing()
            {
                Name = node.Path,
                Alias = node.Alias
            };
        }

        public CppCode Visit(FunctionNode node)
        {
            _rcall = false;

            var func = new CppFunction()
            {
                Name = node.Name,
                ReturnType = (CppElement)Visit(node.ReturnType),
                Code = (CppBlock)Visit(node.Code)
            };

            foreach (var p in node.Parameters)
                func.Parameters.Add((CppElement)Visit(p));

            return func;
        }

        public CppCode Visit(TypeNode node)
        {
            _rcall = false;

            var element = new CppElement();

            element.Parts.Add(new CppUnit(GetCppTypeName(node.TypePath)));
            if (node.IsConstant)
                element.Parts.Add(new CppUnit("const"));

            if (node.IsArrayType || node.IsReference)
            {
                element.Parts.Add(new CppUnit("*"));
                element.Parts.AddRange(((CppElement)Visit(node.ReferenceType)).Parts);
            }

            return element;
        }

        private static string GetCppTypeName(string typeName)
        {
            switch (typeName)
            {
                case "~auto": return "auto";

                case "string": return "std::string";

                case "byte":  return "uint8_t";
                case "short": return "uint16_t";
                case "int":   return "int32_t";
                case "long":  return "int64_t";

                default: return typeName;
            }
        }

        public CppCode Visit(VariableDeclarationNode node)
        {
            _rcall = false;

            var declaration = new CppElement();

            declaration.Parts.AddRange(((CppElement)Visit(node.Type)).Parts);
            declaration.Parts.Add(new CppUnit(" "));
            declaration.Parts.Add(new CppUnit(node.Name));

            return declaration;
        }

        public CppCode Visit(BlockNode node)
        {
            _rcall = false;

            var code = new CppBlock();

            foreach (var line in node.Code)
                code.Elements.Add(Visit(line));

            return code;
        }

        public CppCode Visit(IfNode node)
        {
            _rcall = false;

            var block = new CppBlockExecutor("if");
            block.Arguments.Add(Visit(node.Condition));

            var code = Visit(node.TrueBlock);
            if (code is CppBlock)
            {
                block.Code = (CppBlock)code;
            }
            else
            {
                block.Code.Elements.Add(code);
            }

            if (node.FalseBlock != null)
            {
                var elseBlock = new CppBlockExecutor("else");
                var falseCode = Visit(node.FalseBlock);

                if (falseCode is CppBlock)
                {
                    elseBlock.Code = (CppBlock)falseCode;
                }
                else
                {
                    elseBlock.Code.Elements.Add(falseCode);
                }

                block.ConnectedExecutor = elseBlock;
            }

            return block;
        }

        public CppCode Visit(BinaryOperatorNode node)
        {
            _rcall = false;

            var code = new CppElement();
            
            if (node.LeftOperand is BinaryOperatorNode)
            {
                code.Parts.Add(new CppUnit("("));
                code.Parts.Add(Visit(node.LeftOperand));
                code.Parts.Add(new CppUnit(")"));
            }
            else
            {
                code.Parts.Add(Visit(node.LeftOperand));
            }

            code.Parts.Add(new CppUnit(" "));
            code.Parts.Add(new CppUnit(node.Operation.Representation));
            code.Parts.Add(new CppUnit(" "));

            if (node.RightOperand is BinaryOperatorNode)
            {
                code.Parts.Add(new CppUnit("("));
                code.Parts.Add(Visit(node.RightOperand));
                code.Parts.Add(new CppUnit(")"));
            }
            else
            {
                code.Parts.Add(Visit(node.RightOperand));
            }

            return code;
        }

        public CppCode Visit(VariableNode node)
        {
            _rcall = false;
            return new CppUnit(node.Name.Replace(":", "::"));
        }

        public CppCode Visit(NumberNode node)
        {
            _rcall = false;
            return new CppUnit(node.NumericValue);
        }

        public CppCode Visit(StringNode node)
        {
            _rcall = false;
            return new CppUnit("std::string(\"" + node.StringValue + "\")");
        }

        public CppCode Visit(FunctionCallNode node)
        {
            _rcall = false;

            var code = new CppElement();
            var callee = node.CalleeExpression;
            if (callee is BinaryOperatorNode)
            {
                code.Parts.Add(new CppUnit("("));
                code.Parts.Add(Visit(callee));
                code.Parts.Add(new CppUnit(")"));
            }
            else
            {
                code.Parts.Add(Visit(callee));
            }

            code.Parts.Add(new CppUnit("("));
            for (int i = 0; i < node.Arguments.Count; ++i)
            {
                if (i != 0) code.Parts.Add(new CppUnit(","));
                code.Parts.Add(Visit(node.Arguments[i]));
            }
            code.Parts.Add(new CppUnit(")"));

            return code;
        }

        public CppCode Visit(ReturnNode node)
        {
            _rcall = false;

            var code = new CppElement();

            code.Parts.Add(new CppUnit("return"));
            code.Parts.Add(new CppUnit(" "));
            code.Parts.Add(Visit(node.Expression));

            return code;
        }
    }
}
