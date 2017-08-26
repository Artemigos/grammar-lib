using System;
using System.Collections.Generic;
using System.Text;

namespace GrammarLib
{
    public static class GrammarBuilder<TToken, TSymbol>
    {
        // token types
        public static IEnumerable<TokenType<TToken>> Tokens(params TokenType<TToken>[] tokens) =>
            tokens;

        public static TokenType<TToken> Token(TToken token, string regex) =>
            new TokenType<TToken>(token, regex);

        // rules
        public static IEnumerable<Rule<TToken, TSymbol>> Rules(params Rule<TToken, TSymbol>[] rules) =>
            rules;

        public static Rule<TToken, TSymbol> Rule(TSymbol symbol, params Ref<TToken, TSymbol>[] structure) =>
            new Rule<TToken, TSymbol>(symbol, structure);

        public static Ref<TToken, TSymbol> T(TToken t) =>
            new Ref<TToken, TSymbol>(t);

        public static Ref<TToken, TSymbol> S(TSymbol s) =>
            new Ref<TToken, TSymbol>(s);

        public static Ref<TToken, TSymbol> E() =>
            new Ref<TToken, TSymbol>();

        // grammar
        public static Grammar<TToken, TSymbol> Grammar(
            IEnumerable<TokenType<TToken>> tokens,
            IEnumerable<Rule<TToken, TSymbol>> rules,
            TSymbol rootSymbol) =>
            new Grammar<TToken, TSymbol>(tokens, rules, rootSymbol);
    }
}
