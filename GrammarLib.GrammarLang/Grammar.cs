using System;

using static GrammarLib.GrammarBuilder<GrammarLib.GrammarLang.GrammarToken, GrammarLib.GrammarLang.GrammarSymbol>;

namespace GrammarLib.GrammarLang
{
    public enum GrammarToken { Identifier, Quot, AssignmentOp, Text, RootOp, Empty, Terminator, Separator }
    public enum GrammarSymbol { Document, Rule, Token, Root, RefList, TokensList, RulesList }

    public static class Grammar
    {
        public static Grammar<GrammarToken, GrammarSymbol> Create() =>
            Grammar(
                Tokens(
                    Token(GrammarToken.Empty, @"\bEMPTY\b"),
                    Token(GrammarToken.Identifier, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b"),
                    Token(GrammarToken.Quot, "\""),
                    Token(GrammarToken.AssignmentOp, ":="),
                    Token(GrammarToken.RootOp, ">"),
                    Token(GrammarToken.Terminator, ";"),
                    Token(GrammarToken.Separator, "---"),
                    Token(GrammarToken.Text, @"[^""\n]+")
                ),
                Rules(
                    Rule(GrammarSymbol.Token, T(GrammarToken.Identifier), T(GrammarToken.AssignmentOp), T(GrammarToken.Quot), T(GrammarToken.Text), T(GrammarToken.Quot), T(GrammarToken.Terminator)),
                    Rule(GrammarSymbol.RefList, T(GrammarToken.Identifier), S(GrammarSymbol.RefList)),
                    Rule(GrammarSymbol.RefList, T(GrammarToken.Identifier)),
                    Rule(GrammarSymbol.Rule, T(GrammarToken.Identifier), T(GrammarToken.AssignmentOp), S(GrammarSymbol.RefList), T(GrammarToken.Terminator)),
                    Rule(GrammarSymbol.Root, T(GrammarToken.RootOp), T(GrammarToken.Identifier), T(GrammarToken.Terminator)),

                    Rule(GrammarSymbol.TokensList, S(GrammarSymbol.Token), S(GrammarSymbol.TokensList)),
                    Rule(GrammarSymbol.TokensList, S(GrammarSymbol.Token)),
                    Rule(GrammarSymbol.RulesList, S(GrammarSymbol.Rule), S(GrammarSymbol.RulesList)),
                    Rule(GrammarSymbol.RulesList, S(GrammarSymbol.Rule)),

                    Rule(GrammarSymbol.Document, S(GrammarSymbol.TokensList), T(GrammarToken.Separator), S(GrammarSymbol.RulesList), S(GrammarSymbol.Root))
                ),
                GrammarSymbol.Document
            );
    }
}
