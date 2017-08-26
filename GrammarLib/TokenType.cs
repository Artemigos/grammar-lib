using System;
using System.Collections.Generic;
using System.Text;

namespace GrammarLib
{
    public class TokenType<TToken>
    {
        public TokenType(TToken type, string regex)
        {
            Type = type;
            Regex = regex;
        }

        public TToken Type { get; }
        public string Regex { get; }
    }
}
