using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrammarLib
{
    public class Tokenizer<TToken>
    {
        private readonly TokenType<TToken>[] _tokenTypes;

        public Tokenizer(IEnumerable<TokenType<TToken>> tokenTypes)
        {
            _tokenTypes = tokenTypes.ToArray();
        }

        public List<Token<TToken>> Tokenize(string data)
        {
            var input = data;
            var result = new List<Token<TToken>>();

            while (input != string.Empty)
            {
                Token<TToken> token;
                (token, input) = ReadToken(input);
                result.Add(token);
            }

            return result;
        }

        private (Token<TToken> token, string newStr) ReadToken(string input)
        {
            foreach (var tt in _tokenTypes)
            {
                var m = Regex.Match(input, $@"\A({tt.Regex})");
                if (m.Success)
                {
                    var val = m.Groups[1].Value;
                    var token = new Token<TToken>(tt.Type, val);
                    var trimmed =  input.Substring(val.Length).Trim();
                    return (token, trimmed);
                }
            }

            throw new FormatException($"Didn't recognize any token in data '{input.Substring(0, 10)}...'");
        }
    }
}
