using System;
using System.Collections.Generic;
using System.Text;

using static System.Console;

namespace GrammarLib
{
    public class PrintVisitor<TToken, TSymbol> : AstNode<TToken, TSymbol>.Visitor<int>
    {
        public void Visit(AstNode<TToken, TSymbol> node) =>
            Visit(node, 0);

        protected override void VisitSymbol(AstNode<TToken, TSymbol> symbol, int context)
        {
            PrintIndent(context);
            PrintOpenParen();
            PrintNodeType("symbol ");
            PrintSymbolType(symbol.Symbol.ToString());
            PrintCloseParen();
            WriteLine();

            base.VisitSymbol(symbol, context + 1);
        }

        protected override void VisitToken(AstNode<TToken, TSymbol> token, int context)
        {
            PrintIndent(context);
            PrintOpenParen();
            PrintNodeType("token ");
            PrintTokenType(token.Token.ToString() + " ");
            PrintTokenValue(token.TokenValue);
            PrintCloseParen();
            WriteLine();
        }

        protected override void VisitEmpty(AstNode<TToken, TSymbol> empty, int context)
        {
            PrintIndent(context);
            PrintOpenParen();
            PrintNodeType("empty");
            PrintCloseParen();
            WriteLine();
        }

        private void PrintIndent(int indent) =>
            InColorScope(
                ConsoleColor.DarkGray,
                () =>
                {
                    for (int i = 0; i < indent; ++i)
                        Write("|   ");
                });

        private void PrintTokenValue(string value) =>
            PrintColoredSpan(ConsoleColor.Red, "'" + value + "'");

        private void PrintSymbolType(string type) =>
            PrintColoredSpan(ConsoleColor.Magenta, type);

        private void PrintTokenType(string type) =>
            PrintColoredSpan(ConsoleColor.Green, type);

        private void PrintNodeType(string type) =>
            PrintColoredSpan(ConsoleColor.Blue, type);

        private void PrintOpenParen() =>
            PrintColoredSpan(ConsoleColor.Gray, "[");

        private void PrintCloseParen() =>
            PrintColoredSpan(ConsoleColor.Gray, "]");

        private void PrintColoredSpan(ConsoleColor color, string text) =>
            InColorScope(color, () => Write(text));

        private void InColorScope(ConsoleColor color, Action op)
        {
            var before = ForegroundColor;
            ForegroundColor = color;
            op();
            ForegroundColor = before;
        }
    }
}
