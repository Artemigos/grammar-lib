using System;
using System.Collections.Generic;
using System.Text;

namespace GrammarLib
{
    public class Ref<TToken, TSymbol>
    {
        public Ref(TToken t)
        {
            Token = t;
            Type = RefType.Token;

            Symbol = default(TSymbol);
        }

        public Ref(TSymbol s)
        {
            Symbol = s;
            Type = RefType.Symbol;

            Token = default(TToken);
        }

        public Ref()
        {
            Type = RefType.Empty;

            Symbol = default(TSymbol);
            Token = default(TToken);
        }

        public RefType Type { get; }
        public TSymbol Symbol { get; }
        public TToken Token { get; }

        public bool IsToken => Type == RefType.Token;
        public bool IsSymbol => Type == RefType.Symbol;
        public bool IsEmpty => Type == RefType.Empty;
    }
}
