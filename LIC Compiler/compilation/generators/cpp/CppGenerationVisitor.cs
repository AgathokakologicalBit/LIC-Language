using LIC.Parsing;
using LIC.Parsing.Nodes;
using LIC_Compiler.language;
using System;
using System.Collections.Generic;

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
            if (node == null)
            {
                Console.WriteLine("Unable to visit unknown node(null)");
                throw new ArgumentNullException("node");
            }

            _rcall = true;
            return Visit((dynamic)node);
        }

        public CppCode Visit(CoreNode node)
        {
            var root = new CppRoot();

            foreach (var useNode in node.UsesNodes)
                root.Usings.Add((CppUsing)Visit(useNode));

            bool hasEntryPoint = false;
            foreach (var funcNode in node.FunctionNodes)
            {
                Console.WriteLine(
                    "[ {0} ] Generating code for function '{1}'",
                    "CodeGen | cpp",
                    funcNode.Name
                );

                if (funcNode.Name.ToLower() == "main")
                {
                    Console.WriteLine(
                        "  {0}({1})",
                        funcNode.Name,
                        String.Join(", ", funcNode.Parameters)
                    );
                    if (funcNode.Parameters.Count == 0)
                        hasEntryPoint = true;

                    if (funcNode.Parameters.Count == 1
                        && funcNode.Parameters[0].Type.Equals(new TypeNode()
                        {
                            IsArrayType = true,
                            IsValueType = true,

                            ReferenceType = new TypeNode()
                            {
                                TypePath = "string"
                            }
                        })
                    )
                    {
                        hasEntryPoint = true;
                        funcNode.Parameters = new List<VariableDeclarationNode>
                        {
                            new VariableDeclarationNode(
                                "argc",
                                new TypeNode() {
                                    IsValueType = true,
                                    TypePath = "int"
                                }
                            ),
                            new VariableDeclarationNode(
                                "argv",
                                new TypeNode() {
                                    IsArrayType = true,
                                    IsReference = true,

                                    ReferenceType = new TypeNode() {
                                        IsArrayType = true,
                                        IsReference = true,

                                        ReferenceType = new TypeNode() {
                                            TypePath = "char"
                                        }
                                    }
                                }
                            )
                        };
                    }
                }

                root.Functions.Add((CppFunction)Visit(funcNode));
            }

            if (!hasEntryPoint)
                throw new EntryPointNotFoundException("main() function not found");

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

            if (!String.IsNullOrEmpty(node.TypePath))
            {
                element.Parts.Add(new CppUnit(GetCppTypeName(node.TypePath)));
            }

            if (node.IsConstant)
                element.Parts.Add(new CppUnit("const"));

            if (node.IsReference)
            {
                element.Parts.AddRange(((CppElement)Visit(node.ReferenceType)).Parts);
                element.Parts.Add(new CppUnit("*"));
            }

            return element;
        }

        private static string GetCppTypeName(string typeName)
        {
            switch (typeName)
            {
                case "~auto": return "auto";

                case "byte": return "std::int8_t";
                case "short": return "std::int16_t";
                case "int": return "std::int32_t";
                case "long": return "std::int64_t";

                default:
                    return typeName.Replace(".", "::");
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

        public CppCode Visit(ForLoopNode node)
        {
            _rcall = false;

            var block = new CppBlockExecutor("for");
            var unit = new CppElement();
            unit.Parts.Add(new CppUnit("it : "));
            unit.Parts.Add(Visit(node.Condition));
            block.Arguments.Add(unit);

            var code = Visit(node.CodeBlock);
            if (code is CppBlock)
            {
                block.Code = (CppBlock)code;
            }
            else
            {
                block.Code.Elements.Add(code);
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

            if (node.Operation.Equals(OperatorList.MemberAccess))
            {
                code.Parts.Add(new CppUnit(node.Operation.Representation));
            }
            else
            {
                code.Parts.Add(new CppUnit(" "));
                code.Parts.Add(new CppUnit(node.Operation.Representation));
                code.Parts.Add(new CppUnit(" "));
            }

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
            return new CppUnit(node.Name);
        }

        public CppCode Visit(NumberNode node)
        {
            _rcall = false;
            return new CppUnit(node.NumericValue);
        }

        public CppCode Visit(CharacterNode node)
        {
            _rcall = false;
            return new CppUnit($"'{node.CharacterValue}'");
        }

        public CppCode Visit(StringNode node)
        {
            _rcall = false;
            return new CppUnit($"std::string(\"{node.StringValue}\")");
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
            for (var i = 0; i < node.Arguments.Count; ++i)
            {
                if (i != 0)
                    code.Parts.Add(new CppUnit(", "));
                code.Parts.Add(Visit(node.Arguments[i]));
            }
            code.Parts.Add(new CppUnit(")"));

            return code;
        }

        public CppCode Visit(ObjectIndexerCallNode node)
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

            code.Parts.Add(new CppUnit("["));
            for (var i = 0; i < node.Arguments.Count; ++i)
            {
                if (i != 0)
                    code.Parts.Add(new CppUnit(", "));
                code.Parts.Add(Visit(node.Arguments[i]));
            }
            code.Parts.Add(new CppUnit("]"));

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
