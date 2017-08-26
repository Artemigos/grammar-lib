using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib
{
    public class Grammar<TToken, TSymbol>
    {
        private readonly Tokenizer<TToken> _tokenizer;
        private readonly Parser<TToken, TSymbol> _parser;
        private readonly Ref<TToken, TSymbol> _rootRef;

        public Grammar(IEnumerable<TokenType<TToken>> tokenTypes, IEnumerable<Rule<TToken, TSymbol>> rules, TSymbol rootSymbol)
        {
            TokenTypes = tokenTypes.ToArray();
            Rules = rules.ToArray();
            RootSymbol = rootSymbol;

            _tokenizer = new Tokenizer<TToken>(TokenTypes);
            _parser = new Parser<TToken, TSymbol>(Rules);
            _rootRef = new Ref<TToken, TSymbol>(RootSymbol);
        }

        public TokenType<TToken>[] TokenTypes { get; }
        public Rule<TToken, TSymbol>[] Rules { get; }
        public TSymbol RootSymbol { get; }

        public AstNode<TToken, TSymbol> Parse(string input)
        {
            var tokens = Tokenize(input);
            return Parse(tokens);
        }

        public List<Token<TToken>> Tokenize(string input) =>
            _tokenizer.Tokenize(input);

        public AstNode<TToken, TSymbol> Parse(TokenStream<TToken> stream)
        {
            var (success, tree) = _parser.TryParse(stream, _rootRef);

            if (!success)
            {
                return null;
            }

            return tree;
        }

        public AstNode<TToken, TSymbol> Parse(IEnumerable<Token<TToken>> tokens) =>
            Parse(new TokenStream<TToken>(tokens));
    }
}
