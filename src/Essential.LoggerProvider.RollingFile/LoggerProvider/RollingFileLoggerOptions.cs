namespace Essential.LoggerProvider
{
    public class RollingFileLoggerOptions
    {
        /// <summary>
        /// Gets or sets the template of the file path to write to. Replacement parameters may be used to construct rolling files.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A rolling log file is achieved by including the date in the filename, so that when the date changes
        /// a different file is used.
        /// </para>
        /// <para>
        /// Available tokens are DateTime (a UTC DateTimeOffset) and LocalDateTime (a local DateTimeOffset), 
        /// as well as ApplicationName, ProcessId, ProcessName and MachineName. These use standard .NET 
        /// format strings, e.g. "Trace{DateTime:yyyyMMddTHH}.log" would generate a different log name
        /// each hour.
        /// </para>
        /// <para>
        /// The default filePathTemplate is "{ApplicationName}-{DateTime:yyyy-MM-dd}.log" (note that it uses
        /// UTC time; you can override to LocalDateTime if you want).
        /// </para>
        /// </remarks>
        public string FilePathTemplate { get; set; } = "{BaseDirectory}/{ApplicationName}-{DateTime:yyyy-MM-dd}.log";

        /// <summary>
        /// Gets or sets a value indicating whether scopes should be included in the message.
        /// Defaults to <c>true</c>.
        /// </summary>
        public bool IncludeScopes { get; set; } = true;

        /// <summary>
        /// Gets or sets value indicating if logger accepts and queues writes.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the token format string to use to display trace output.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See TraceFormatter for details of the supported formats.
        /// </para>
        /// <para>
        /// The default value is "{DateTime:u} [{Thread}] {EventType} {Source} {Id}: {Message}{Data}".
        /// </para>
        /// <para>
        /// To get a format that matches Microsoft.VisualBasic.Logging.FileLogTraceListener, 
        /// use the tab delimited format "{Source}&#x9;{EventType}&#x9;{Id}&#x9;{Message}{Data}". 
        /// (Note: In the config XML file the TAB characters are XML encoded; if specifying 
        /// in C# code the delimiters  would be '\t'.)
        /// </para>
        /// <para>
        /// To get a format matching FileLogTraceListener with all TraceOutputOptions enabled, use
        /// "{Source}&#x9;{EventType}&#x9;{Id}&#x9;{Message}&#x9;{Callstack}&#x9;{LogicalOperationStack}&#x9;{DateTime:u}&#x9;{ProcessId}&#x9;{ThreadId}&#x9;{Timestamp}&#x9;{MachineName}".
        /// </para>
        /// <para>
        /// To get a format similar to that produced by System.Diagnostics.TextWriterTraceListener,
        /// use "{Source} {EventType}: {Id} : {Message}{Data}".
        /// </para>
        /// </remarks>
        public string Template { get; set; } =
            "{DateTime:u} [{ThreadId}] {LogLevel} {CategoryName} {Id}: {Message}{ScopeList} {Exception}";
    }
}
