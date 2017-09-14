﻿using LIC.Parsing.Nodes;
using LIC.Tokenization;

namespace LIC.Parsing.ContextParsers
{
    public static class ModuleParser
    {
        public static CoreNode Parse(Parser.State state)
        {
            var coreNode = new CoreNode();

            while (!state.IsErrorOccured())
            {
                if (!ParseUse(state, coreNode)
                    && !ParseFunctionDeclaration(state, coreNode))
                { break; }
            }

            return coreNode;
        }

        private static bool ParseUse(Parser.State state, CoreNode coreNode)
        {
            if (state.IsErrorOccured()) { return false; }
            if (state.GetToken().Type != TokenType.CompilerDirective) { return false; }
            if (state.GetToken().Value != "#use") { return false; }

            state.GetNextNEToken();
            
            string usePath = TypeParser.ParsePath(state);
            string useAlias = null;

            if (state.IsErrorOccured()) { return false; }

            if (state.GetToken().Is(TokenType.Identifier, "as"))
            {
                state.GetNextNEToken();
                useAlias = TypeParser.ParsePath(state);

                if (state.IsErrorOccured()) { return false; }
            }

            coreNode.UsesNodes.Add(new UseNode(usePath, useAlias));
            return true;
        }

        private static bool ParseFunctionDeclaration(Parser.State state, CoreNode coreNode)
        {
            state.Save();

            if (state.GetToken().Type != TokenType.Identifier)
            {
                state.Restore();
                return false;
            }

            string name = state.GetTokenAndMoveNE().Value;

            if (!state.GetToken().Is(TokenSubType.Colon))
            {
                state.ErrorCode = (uint)ErrorCodes.P_ColonBeforeTypeSpeceficationNotFound;
                state.ErrorMessage =
                    $"Expected <Colon>(:) before function type specification " +
                    $"but <{state.GetToken().SubType}>({state.GetToken().Value}) were given";
                return false;
            }
            state.GetNextNEToken();

            TypeNode type;
            if (state.GetToken().SubType == TokenSubType.Colon)
            {
                type = TypeNode.AutoType;
                state.GetNextNEToken();
            }
            else
            {
                type = TypeParser.Parse(state);
            }
            if (state.IsErrorOccured()) { return false; }

            /*
            // Only for class methods
            bool isInstance = state.GetToken().Is(TokenSubType.Dot, ".");
            bool isStatic = state.GetToken().Is(TokenSubType.Colon, ":");

            if (!(isStatic || isInstance))
            {
                state.ErrorCode = (uint)ErrorCodes.P_MethodTypeExpected;
                state.ErrorMessage =
                    $"Expected '.' or ':' as type modifier, but " +
                    $"<{state.GetToken().SubType.ToString()}> were found";
                return false;
            }
            */

            FunctionNode function = new FunctionNode
            {
                Parent = coreNode,
                Name = name,
                Type = FunctionNode.EType.Static,

                ReturnType = type
            };

            coreNode.FunctionNodes.Add(function);

            if (!state.GetToken().Is(TokenSubType.BraceRoundLeft))
            {
                state.ErrorCode = (uint)ErrorCodes.P_OpeningBracketExpected;
                state.ErrorMessage =
                    $"Opening round bracket is expected before parameters list " +
                    $"but <{state.GetToken().SubType}> were given";

                return false;
            }

            state.GetNextNEToken();
            FunctionParser.ParseParametersList(state, function);
            if (state.IsErrorOccured()) { return true; }

            // FunctionParser.ParseTemplateValues(state, function);
            function.Code = CodeParser.Parse(state);
            return !state.IsErrorOccured();
        }
    }
}
