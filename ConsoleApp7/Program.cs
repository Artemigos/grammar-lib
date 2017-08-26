using GrammarLib;
using System;
using System.Collections.Generic;

using static GrammarLib.GrammarBuilder<ConsoleApp7.Program.TokenType, ConsoleApp7.Program.SymbolType>;

namespace ConsoleApp7
{
    public class Program
    {
        public enum TokenType { Def, End, Identifier, Integer, OParen, CParen, Comma }
        public enum SymbolType { FuncDef, ParamList, Param, BodyExpr, VarExpr, IntExpr, CallExpr, CallParams }

        public static Grammar<TokenType, SymbolType> FuncGrammar() =>
            Grammar(
                Tokens(
                    Token(TokenType.Def, @"\bdef\b"),
                    Token(TokenType.End, @"\bend\b"),
                    Token(TokenType.Identifier, @"\b[a-zA-Z]+\b"),
                    Token(TokenType.Integer, @"\b\d+\b"),
                    Token(TokenType.OParen, @"\("),
                    Token(TokenType.CParen, @"\)"),
                    Token(TokenType.Comma, @",")
                ),
                Rules(
                    Rule(SymbolType.IntExpr, T(TokenType.Integer)),
                    Rule(SymbolType.VarExpr, T(TokenType.Identifier)),
                    Rule(SymbolType.CallExpr, T(TokenType.Identifier), T(TokenType.OParen), S(SymbolType.CallParams), T(TokenType.CParen)),
                    Rule(SymbolType.CallParams, S(SymbolType.BodyExpr), T(TokenType.Comma), S(SymbolType.CallParams)),
                    Rule(SymbolType.CallParams, S(SymbolType.BodyExpr)),
                    Rule(SymbolType.CallParams, E()),
                    Rule(SymbolType.BodyExpr, S(SymbolType.IntExpr)),
                    Rule(SymbolType.BodyExpr, S(SymbolType.CallExpr)),
                    Rule(SymbolType.BodyExpr, S(SymbolType.VarExpr)),

                    Rule(SymbolType.Param, T(TokenType.Identifier)),
                    Rule(SymbolType.ParamList, S(SymbolType.Param), T(TokenType.Comma), S(SymbolType.ParamList)),
                    Rule(SymbolType.ParamList, S(SymbolType.Param)),
                    Rule(SymbolType.ParamList, E()),
                    Rule(SymbolType.FuncDef, T(TokenType.Def), T(TokenType.Identifier), T(TokenType.OParen), S(SymbolType.ParamList), T(TokenType.CParen), S(SymbolType.BodyExpr), T(TokenType.End))
                ),
                SymbolType.FuncDef
            );

        static void Main(string[] args)
        {
            var grammar = FuncGrammar();
            var testCase = "def f(x, y) g(x, h(1, y), i()) end";
            //VerboseTest(grammar, testCase);
            //SimpleTest(grammar, testCase);

            Console.ReadKey(true);
        }

        private static void GrammarParsingTest(string grammarText)
        {
            var grammar = 
        }

        private static void SimpleTest(Grammar<TokenType, SymbolType> grammar, string testCase)
        {
            var ast = grammar.Parse(testCase);

            if (ast == null)
            {
                Console.WriteLine("Parsing failed.");
            }
            else
            {
                var printer = new PrintVisitor<TokenType, SymbolType>();
                printer.Visit(ast);
            }
        }

        private static void VerboseTest(Grammar<TokenType, SymbolType> grammar, string testCase)
        {
            List<Token<TokenType>> tokens = null;

            try
            {
                tokens = grammar.Tokenize(testCase);
                Console.WriteLine("Tokenizing phase:");
                Console.WriteLine(string.Join("\n", tokens));
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine();

            var ast = grammar.Parse(tokens);

            if (ast == null)
            {
                Console.WriteLine("Parsing failed.");
            }
            else
            {
                Console.WriteLine("Parsing succeeded:");
                var printer = new PrintVisitor<TokenType, SymbolType>();
                printer.Visit(ast);
            }
        }
    }
}
