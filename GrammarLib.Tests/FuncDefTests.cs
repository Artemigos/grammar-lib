using System;
using System.Collections.Generic;
using Xunit;

using static GrammarLib.GrammarBuilder<GrammarLib.Tests.FuncDefTests.TokenType, GrammarLib.Tests.FuncDefTests.SymbolType>;

namespace GrammarLib.Tests
{
    public class FuncDefTests
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

        [Fact]
        public void EverythingAtOnceTest()
        {
            var grammar = FuncGrammar();
            var input = "def f(x, y) g(x, h(1, y), i()) end";
            var ast = grammar.Parse(input);

            Assert.NotNull(ast);
        }

        [Fact]
        public void StepByStepTest()
        {
            var grammar = FuncGrammar();
            var input = "def f(x, y) g(x, h(1, y), i()) end";
            List<Token<TokenType>> tokens = null;

            tokens = grammar.Tokenize(input);
            var ast = grammar.Parse(tokens);

            Assert.NotNull(ast);
        }
    }
}
