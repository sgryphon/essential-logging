namespace Essential
{
    public interface ITemplateValueProvider
    {
        bool TryGetArgumentValue(string name, out object? value);
    }
}
