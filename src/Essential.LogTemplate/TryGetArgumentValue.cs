namespace Essential
{
    /// <summary>
    /// Provides named argument values for the StringTemplate; the TryGetValue() method of IDictionary<string, object> matches this delegate.
    /// </summary>
    /// <param name="name">Name of the argument required.</param>
    /// <param name="value">Value of the argument, if it exists.</param>
    /// <returns>true if the argument name is valid, i.e. the value can be supplied; false if the argument name is invalid (usually treated as an error)</returns>
    public delegate bool TryGetArgumentValue(string name, out object? value);
}
