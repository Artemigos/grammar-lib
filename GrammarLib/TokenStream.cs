using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib
{
    public class TokenStream<TToken>
    {
        private readonly Token<TToken>[] _tokens;

        private int _head;

        public TokenStream(IEnumerable<Token<TToken>> tokens)
        {
            _tokens = tokens.ToArray();
            _head = 0;

            if (_tokens.Length == 0)
            {
                throw new ArgumentException("Token stream can't be empty.", nameof(tokens));
            }
        }

        public bool EOS => _head >= _tokens.Length;

        public Token<TToken> Consume() =>
            _tokens[_head++];

        public Token<TToken> Peek(int ahead = 0) =>
            _head + ahead < _tokens.Length ?
                _tokens[_head + ahead] :
                null;

        public int Snapshot() =>
            _head;

        public void RestoreSnapshot(int snapshot) =>
            _head = snapshot;
    }
}
