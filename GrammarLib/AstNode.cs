using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib
{
    public class AstNode<TToken, TSymbol> : Ref<TToken, TSymbol>
    {
        public abstract class Visitor<TContext>
        {
            public virtual void Visit(AstNode<TToken, TSymbol> node, TContext context)
            {
                if (node.IsSymbol)
                {
                    VisitSymbol(node, context);
                }
                else if (node.IsToken)
                {
                    VisitToken(node, context);
                }
                else
                {
                    VisitEmpty(node, context);
                }
            }

            protected virtual void VisitSymbol(AstNode<TToken, TSymbol> symbol, TContext context)
            {
                foreach (var sub in symbol.Subnodes)
                {
                    Visit(sub, context);
                }
            }

            protected virtual void VisitToken(AstNode<TToken, TSymbol> token, TContext context)
            {
            }

            protected virtual void VisitEmpty(AstNode<TToken, TSymbol> empty, TContext context)
            {
            }
        }

        public AstNode(Token<TToken> t)
            : base(t.Type)
        {
            Subnodes = null;
            TokenValue = t.Value;
        }

        public AstNode(TSymbol s, IEnumerable<AstNode<TToken, TSymbol>> subnodes)
            : base(s)
        {
            Subnodes = subnodes.ToArray();
            TokenValue = null;
        }

        public AstNode()
            : base()
        {
            Subnodes = null;
            TokenValue = null;
        }

        public AstNode<TToken, TSymbol>[] Subnodes { get; }

        public string TokenValue { get; }
    }
}
