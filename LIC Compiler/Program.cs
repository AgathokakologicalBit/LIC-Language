using LIC.Parsing;
using LIC.Parsing.Nodes;
using LIC.Tokenization;
using LIC_Compiler.compilation.generators.cpp;
using System;
using System.IO;

namespace LIC
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Target file is required");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine(
                     "Error #LC001:\n" +
                    $"File {args[0]} does not exists:\n" +
                    $"  {Path.GetFullPath(args[0])}"
                );
                return;
            }

            var code = File.ReadAllText(args[0]);
            var tokenizer = new Tokenizer(code, new TokenizerOptions
            {
                SkipWhitespace = true
            });

            var tokens = tokenizer.Tokenize();
            var ast = Parser.Parse(tokens);
            if (ast == null)
            {
                Console.Error.WriteLine("Error occurred during compilation process");
                return;
            }


            Console.WriteLine("\n===---   STATS   ---===");

            Console.WriteLine($" [T] Tokens count: {tokens.Length}");

            Console.WriteLine($" [P] Uses({ast.UsesNodes.Count}):");
            Console.WriteLine(String.Join("\n", ast.UsesNodes));

            Console.WriteLine($" [P] Classes({ast.ClassNodes.Count}):");
            Console.WriteLine(String.Join("\n\n", ast.ClassNodes));

            Console.WriteLine($" [P] Functions({ast.FunctionNodes.Count}):");
            Console.WriteLine(String.Join("\n\n", ast.FunctionNodes));


            // Console.WriteLine("\n\n===---    AST    ---===");
            File.WriteAllText(
                Path.GetFileNameWithoutExtension(args[0]) + "_gen.cpp",
                new CppGenerationVisitor().Visit(ast).ToString()
            );

            Console.WriteLine("Successfull compilation");
        }
    }
}
