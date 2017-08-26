using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib
{
    public class Rule<TToken, TSymbol>
    {
        public Rule(TSymbol symbol, IEnumerable<Ref<TToken, TSymbol>> structure)
        {
            Symbol = symbol;
            Structure = structure.ToArray();
        }

        public TSymbol Symbol { get; }
        public Ref<TToken, TSymbol>[] Structure { get; }
    }
}
