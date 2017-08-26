namespace GrammarLib
{
    [System.Diagnostics.DebuggerDisplay("[{Type} {Value}]")]
    public class Token<TToken>
    {
        public Token(TToken type, string value)
        {
            Type = type;
            Value = value;
        }

        public TToken Type { get; }
        public string Value { get; }

        public override string ToString() =>
            $"[{Type} '{Value}']";
    }
}