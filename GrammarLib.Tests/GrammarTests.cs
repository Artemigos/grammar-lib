using System;
using System.Linq;
using Xunit;
using GrammarLib.GrammarLang;

namespace GrammarLib.Tests
{
    public class GrammarTests
    {
        private GrammarToken I => GrammarToken.Identifier;
        private GrammarToken Op => GrammarToken.AssignmentOp;
        private GrammarToken Q => GrammarToken.Quot;
        private GrammarToken T => GrammarToken.Text;
        private GrammarToken R => GrammarToken.RootOp;
        private GrammarToken X => GrammarToken.Terminator;
        private GrammarToken S => GrammarToken.Separator;

        [Fact]
        public void ReadsTokens()
        {
            var grammar = Grammar.Create();
            var test = @"
INT    := ""\b\d+\b"";
OP     := ""[+-*/]"";
OPAREN := ""\("";
CPAREN := ""\)"";

---

EXPR := EXPR OP EXPR;
EXPR := OPAREN EXPR CPAREN;
EXPR := INT;

> EXPR;
";

            var tokens = grammar.Tokenize(test);
            var types = tokens.Select(x => x.Type).ToArray();
            var values = tokens.Select(x => x.Value).ToArray();

            var expectedTypes = new[] { I, Op, Q, T, Q, X, I, Op, Q, T, Q, X, I, Op, Q, T, Q, X, I, Op, Q, T, Q, X, S, I, Op, I, I, I, X, I, Op, I, I, I, X, I, Op, I, X, R, I, X };
            var expectedValues = new[] { "INT", ":=", "\"", @"\b\d+\b", "\"", ";", "OP", ":=", "\"", "[+-*/]", "\"", ";", "OPAREN", ":=", "\"", @"\(", "\"", ";", "CPAREN", ":=", "\"", @"\)", "\"", ";",
                "---", "EXPR", ":=", "EXPR", "OP", "EXPR", ";", "EXPR", ":=", "OPAREN", "EXPR", "CPAREN", ";", "EXPR", ":=", "INT", ";", ">", "EXPR", ";" };

            Assert.Equal(expectedTypes, types);
            Assert.Equal(expectedValues, values);
        }
    }
}
