using System;
using System.Collections.Generic;
using GrammarLib;
using GrammarLib.GrammarLang;

namespace ConsoleApp7
{
    public class Program
    {

        static void Main(string[] args)
        {
            GrammarParsingTest();

            if (!Console.IsInputRedirected)
                Console.ReadKey(true);
        }

        private static void GrammarParsingTest()
        {
            var grammar = Grammar.Create();
            var test = @"
INT    := ""\b\d+\b"";
OP     := ""[+\-*/]"";
OPAREN := ""\("";
CPAREN := ""\)"";

---

EXPR := OPAREN EXPR CPAREN OP EXPR;
EXPR := INT OP EXPR;
EXPR := OPAREN EXPR CPAREN;
EXPR := INT;

> EXPR;
";
            var tokens = grammar.Tokenize(test);
            Console.WriteLine(string.Join("\n", tokens));

            var result = grammar.Parse(tokens);

            if (result != null)
            {
                var printer = new PrintVisitor<GrammarToken, GrammarSymbol>();
                printer.Visit(result);
            }

            Console.WriteLine("Now running the resultin grammar:");

            var reader = new Reader();
            var resultGrammar = reader.Read(result);

            var test2 = "(4 + (2 * 8)) / (6 - 1)";
            Console.WriteLine("Input: " + test2);

            var tokens2 = resultGrammar.Tokenize(test2);
            Console.WriteLine(string.Join("\n", tokens2));

            var result2 = resultGrammar.Parse(tokens2);

            if (result2 != null)
            {
                var printer = new PrintVisitor<string, string>();
                printer.Visit(result2);
            }
        }
    }
}
