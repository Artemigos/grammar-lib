using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib
{
    public class Parser<TToken, TSymbol>
    {
        private static readonly (bool, AstNode<TToken, TSymbol>) Fail = (false, null);

        private readonly Rule<TToken, TSymbol>[] rules;

        private readonly IEqualityComparer<TToken> _tEq;

        private readonly IEqualityComparer<TSymbol> _sEq;

        public Parser(IEnumerable<Rule<TToken, TSymbol>> rules)
        {
            this.rules = rules.ToArray();

            _tEq = EqualityComparer<TToken>.Default;
            _sEq = EqualityComparer<TSymbol>.Default;
        }

        public (bool success, AstNode<TToken, TSymbol> result) TryParse(TokenStream<TToken> stream, Ref<TToken, TSymbol> root)
        {
            var result = TryParseAny(stream, root);
            return result.success && stream.EOS ? result : Fail;
        }

        private (bool success, AstNode<TToken, TSymbol> result) TryParseAny(TokenStream<TToken> stream, Ref<TToken, TSymbol> root)
        {
            if (root.IsToken)
            {
                return TryParseToken(stream, root.Token);
            }
            else if (root.IsSymbol)
            {
                return TryParseSymbol(stream, root.Symbol);
            }
            else
            {
                return (true, new AstNode<TToken, TSymbol>());
            }
        }

        private (bool success, AstNode<TToken, TSymbol> result) TryParseSymbol(TokenStream<TToken> stream, TSymbol symbol)
        {
            foreach (var r in rules.Where(x => _sEq.Equals(x.Symbol, symbol)))
            {
                var snapshot = stream.Snapshot();
                bool passedAll = true;
                List<AstNode<TToken, TSymbol>> subnodes = new List<AstNode<TToken, TSymbol>>();

                foreach (var s in r.Structure)
                {
                    var (success, subnode) = TryParseAny(stream, s);
                    if (!success)
                    {
                        passedAll = false;
                        break;
                    }

                    subnodes.Add(subnode);
                }

                if (passedAll)
                {
                    return (true, new AstNode<TToken, TSymbol>(symbol, subnodes));
                }
                else
                {
                    stream.RestoreSnapshot(snapshot);
                }
            }

            return Fail;
        }

        private (bool success, AstNode<TToken, TSymbol> result) TryParseToken(TokenStream<TToken> stream, TToken token)
        {
            if (_tEq.Equals(stream.Peek().Type, token))
            {
                var t = stream.Consume();
                return (true, new AstNode<TToken, TSymbol>(t));
            }

            return Fail;
        }
    }
}
