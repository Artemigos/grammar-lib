using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarLib.GrammarLang
{
    public class Reader : AstNode<GrammarToken, GrammarSymbol>.Visitor<List<string>>
    {
        public Grammar<string, string> Read(AstNode<GrammarToken, GrammarSymbol> grammarTree) =>
            VisitDocument(grammarTree, new List<string>());

        protected override void VisitSymbol(AstNode<GrammarToken, GrammarSymbol> symbol, List<string> context)
        {
            switch (symbol.Symbol)
            {
                case GrammarSymbol.Document:
                    VisitDocument(symbol, context);
                    break;
                case GrammarSymbol.Rule:
                    VisitRule(symbol, context);
                    break;
                case GrammarSymbol.Token:
                    VisitTokenType(symbol, context);
                    break;
                case GrammarSymbol.Root:
                    VisitRoot(symbol, context);
                    break;
                case GrammarSymbol.RefList:
                    VisitRefList(symbol, context);
                    break;
                case GrammarSymbol.TokensList:
                    VisitTokensList(symbol, context);
                    break;
                case GrammarSymbol.RulesList:
                    VisitRulesList(symbol, context);
                    break;
                default:
                    base.Visit(symbol, context);
                    break;
            }
        }

        protected virtual Grammar<string, string> VisitDocument(AstNode<GrammarToken, GrammarSymbol> document, List<string> context)
        {
            var tokens = document.Subnodes.FindSymbol(GrammarSymbol.TokensList);
            var rules = document.Subnodes.FindSymbol(GrammarSymbol.RulesList);
            var root = document.Subnodes.FindSymbol(GrammarSymbol.Root);

            var t = VisitTokensList(tokens, context);
            var r = VisitRulesList(rules, context);
            var ro = VisitRoot(root, context);

            return new Grammar<string, string>(t, r, ro);
        }

        protected virtual Rule<string, string> VisitRule(AstNode<GrammarToken, GrammarSymbol> rule, List<string> context)
        {
            var name = rule.Subnodes.FindToken();
            var refs = rule.Subnodes.FindSymbol(GrammarSymbol.RefList);

            var r = VisitRefList(refs, context);

            return new Rule<string, string>(name.TokenValue, r);
        }

        protected virtual TokenType<string> VisitTokenType(AstNode<GrammarToken, GrammarSymbol> tokenType, List<string> context)
        {
            var name = tokenType.Subnodes.FindToken().TokenValue;
            var regex = tokenType.Subnodes.FindToken(GrammarToken.Text);

            if (context.Contains(name))
            {
                throw new FormatException("Token type can't be defined twice.");
            }

            context.Add(name);

            return new TokenType<string>(name, regex.TokenValue);
        }

        protected virtual string VisitRoot(AstNode<GrammarToken, GrammarSymbol> root, List<string> context)
        {
            var r = root.Subnodes.FindToken(GrammarToken.Identifier);

            return r.TokenValue;
        }

        protected virtual IEnumerable<Ref<string, string>> VisitRefList(AstNode<GrammarToken, GrammarSymbol> refList, List<string> context)
        {
            var id = refList.Subnodes.FindToken(GrammarToken.Identifier).TokenValue;
            var subrefs = refList.Subnodes.FindOptionalSymbol(GrammarSymbol.RefList);

            Ref<string, string> r;

            if (context.Contains(id))
            {
                r = new Ref<string, string>(t: id);
            }
            else
            {
                r = new Ref<string, string>(s: id);
            }

            if (subrefs == null)
            {
                return new[] { r };
            }

            var s = VisitRefList(subrefs, context);
            var result = new List<Ref<string, string>>(s);
            result.Insert(0, r);

            return result;
        }

        protected virtual IEnumerable<TokenType<string>> VisitTokensList(AstNode<GrammarToken, GrammarSymbol> tokensList, List<string> context)
        {
            var token = tokensList.Subnodes.FindSymbol(GrammarSymbol.Token);
            var subtokens = tokensList.Subnodes.FindOptionalSymbol(GrammarSymbol.TokensList);

            var t = VisitTokenType(token, context);

            if (subtokens == null)
            {
                return new[] { t };
            }

            var s = VisitTokensList(subtokens, context);
            var result = new List<TokenType<string>>(s);
            result.Insert(0, t);

            return result;
        }

        protected virtual IEnumerable<Rule<string, string>> VisitRulesList(AstNode<GrammarToken, GrammarSymbol> rulesList, List<string> context)
        {
            var rule = rulesList.Subnodes.FindSymbol(GrammarSymbol.Rule);
            var subrules = rulesList.Subnodes.FindOptionalSymbol(GrammarSymbol.RulesList);

            var r = VisitRule(rule, context);

            if (subrules == null)
            {
                return new[] { r };
            }

            var s = VisitRulesList(subrules, context);
            var result = new List<Rule<string, string>>(s);
            result.Insert(0, r);

            return result;
        }
    }
}
