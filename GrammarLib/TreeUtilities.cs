using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib
{
    public static class TreeUtilities
    {
        public static AstNode<TToken, TSymbol> FindSymbol<TToken, TSymbol>(this IEnumerable<AstNode<TToken, TSymbol>> subnodes, TSymbol symbol) =>
            subnodes.First(x => x.IsSymbol && EqualityComparer<TSymbol>.Default.Equals(x.Symbol, symbol));

        public static AstNode<TToken, TSymbol> FindToken<TToken, TSymbol>(this IEnumerable<AstNode<TToken, TSymbol>> subnodes, TToken token) =>
            subnodes.First(x => x.IsToken && EqualityComparer<TToken>.Default.Equals(x.Token, token));

        public static AstNode<TToken, TSymbol> FindOptionalSymbol<TToken, TSymbol>(this IEnumerable<AstNode<TToken, TSymbol>> subnodes, TSymbol symbol) =>
            subnodes.FirstOrDefault(x => x.IsSymbol && EqualityComparer<TSymbol>.Default.Equals(x.Symbol, symbol));

        public static AstNode<TToken, TSymbol> FindOptionalToken<TToken, TSymbol>(this IEnumerable<AstNode<TToken, TSymbol>> subnodes, TToken token) =>
            subnodes.FirstOrDefault(x => x.IsToken && EqualityComparer<TToken>.Default.Equals(x.Token, token));

        public static AstNode<TToken, TSymbol> FindSymbol<TToken, TSymbol>(this IEnumerable<AstNode<TToken, TSymbol>> subnodes) =>
            subnodes.First(x => x.IsSymbol);

        public static AstNode<TToken, TSymbol> FindToken<TToken, TSymbol>(this IEnumerable<AstNode<TToken, TSymbol>> subnodes) =>
            subnodes.First(x => x.IsToken);
    }
}
