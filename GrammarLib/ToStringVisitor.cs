using System.Text;

namespace GrammarLib
{
    public class ToStringVisitor<TToken, TSymbol> : AstNode<TToken, TSymbol>.Visitor<(StringBuilder, int)>
    {
        public string BuildString(AstNode<TToken, TSymbol> node)
        {
            var builder = new StringBuilder();
            Visit(node, (builder, 0));
            return builder.ToString();
        }

        protected override void VisitSymbol(AstNode<TToken, TSymbol> symbol, (StringBuilder, int) context)
        {
            PrintIndent(context.Item1, context.Item2);
            context.Item1.AppendLine($"[symbol {symbol.Symbol}]");

            base.VisitSymbol(symbol, (context.Item1, context.Item2 + 1));
        }

        protected override void VisitToken(AstNode<TToken, TSymbol> token, (StringBuilder, int) context)
        {
            PrintIndent(context.Item1, context.Item2);
            context.Item1.AppendLine($"[token {token.Token} '{token.TokenValue}']");
        }

        protected override void VisitEmpty(AstNode<TToken, TSymbol> empty, (StringBuilder, int) context)
        {
            PrintIndent(context.Item1, context.Item2);
            context.Item1.AppendLine("[empty]");
        }

        private void PrintIndent(StringBuilder builder, int indent)
        {
            for (int i = 0; i < indent; ++i)
                builder.Append("|   ");
        }
    }
}
