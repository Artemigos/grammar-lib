using System;

using static GrammarLib.GrammarBuilder<GrammarLib.GrammarLang.GrammarToken, GrammarLib.GrammarLang.GrammarSymbol>;

namespace GrammarLib.GrammarLang
{
    public enum GrammarToken { Identifier, Quot, AssignmentOp, Text, RootOp }
    public enum GrammarSymbol { Document }

    public static class Grammar
    {
        public static Grammar<GrammarToken, GrammarSymbol> Create() =>
            Grammar(
                Tokens(
                    Token(GrammarToken.Identifier, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b"),
                    Token(GrammarToken.Quot, "\""),
                    Token(GrammarToken.AssignmentOp, ":="),
                    Token(GrammarToken.RootOp, ">"),
                    Token(GrammarToken.Text, @"[^""\n]+")
                ),
                Rules(
                    Rule(GrammarSymbol.Document, E())
                ),
                GrammarSymbol.Document
            );
    }
}
