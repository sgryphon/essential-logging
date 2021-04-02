namespace DiagnosticSource.Library5
{
    public class NextPrimeDiagnostic
    {
        public int Index { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"Prime[{Index}]={Value}";
        }
    }
}
