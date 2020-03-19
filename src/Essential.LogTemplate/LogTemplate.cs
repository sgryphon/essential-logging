using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Essential
{
    /// <summary>
    /// Formats trace output using a template.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Uses the StringTemplate.Format function to format trace output using a supplied template
    /// and trace information. The formatted event can then be written to the console, a
    /// file, or other text-based output.
    /// </para>
    /// <para>
    /// The following parameters are available in the template string:
    /// Data, Data0, EventType, Id, Message, ActivityId, RelatedActivityId, Source, 
    /// Callstack, DateTime (or UtcDateTime), LocalDateTime, LogicalOperationStack, 
    /// ProcessId, ThreadId, Timestamp, MachineName, ProcessName, ThreadName,
    /// ApplicationName, MessagePrefix, AppDomain.
    /// </para>
    /// <para>
    /// An example template that generates the same output as the ConsoleListener is:
    /// "{Source} {EventType}: {Id} : {Message}".
    /// </para>
    /// </remarks>
    public class LogTemplate
    {
        private readonly SystemValueProvider _systemValueProvider;
        private static readonly Regex controlCharRegex = new Regex(@"\p{C}", RegexOptions.Compiled);
        private const int DefaultFacility = 16;
        private const int MaxPrefixLength = 40;
        private const string PrefixContinuation = "...";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template">Log output template</param>
        public LogTemplate(string template) : this(DefaultFacility, template) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="template"></param>
        public LogTemplate(int facility, string template)
        {
            if (facility < 0 || facility > 23)
                throw new ArgumentOutOfRangeException(nameof(facility), facility,
                    "Facility values must be in the range of 0 to 23 inclusive");
            if (template == null) throw new ArgumentNullException(nameof(template));

            Facility = facility;
            Template = template;
            _systemValueProvider = new SystemValueProvider();
        }

        public int Facility { get; }

        public string Template { get; }

        public string Bind(string categoryName, LogLevel logLevel, EventId? eventId, string message,
            Exception? exception, object[]? scopes)
        {
            TryGetArgumentValue valueProvider = delegate(string name, out object? value)
            {
                switch (name.ToUpperInvariant())
                {
                    case "EVENTTYPE":
                    case "LOGLEVEL":
                        value = logLevel;
                        break;
                    case "FACILITY":
                        value = Facility;
                        break;
                    case "SEVERITY":
                        value = GetSeverity(logLevel);
                        break;
                    case "PRI":
                    case "PRIORITY":
                        value = Facility * 8 + GetSeverity(logLevel);
                        break;
                    case "ID":
                        value = eventId?.Id;
                        break;
                    case "EVENTID":
                        value = eventId?.ToString();
                        break;
                    case "MESSAGE":
                        value = message;
                        break;
                    case "MESSAGEPREFIX":
                        value = FormatPrefix(message);
                        break;
                    case "SOURCE":
                    case "CATEGORYNAME":
                        value = categoryName;
                        break;
                    case "EXCEPTIONMESSAGE":
                        value = exception?.Message;
                        break;
                    case "EXCEPTION":
                        value = exception;
                        break;
                    case "SCOPES":
                        value = FormatScopes(scopes);
                        break;
                    case "SCOPELIST":
                        value = FormatScopes(scopes, " => ", " => ");
                        break;
                    default:
                        if (!_systemValueProvider.TryGetArgumentValue(name, out value))
                        {
                            value = "{" + name + "}";
                        }

                        break;
                }

                return true;
            };
            var result = StringTemplate.Format(Template, valueProvider);
            return result;
        }

        private static string FormatPrefix(string message)
        {
            // TODO: Add a FormattableString class that implements IFormattable that applies formats to strings to truncate at given length,
            // e.g. string.Format("Message {0:t20}", message), string.Format("Message {0:s0,40}"), string.Format("Message {0:e0,40}")
            // ideas - truncate, cut, ellipses, substring, prefix.
            if (!string.IsNullOrEmpty(message))
            {
                // Prefix is the part of the message before the first <;,.:>
                string[] split = message.Split(new char[] {'.', '!', '?', ':', ';', ',', '\r', '\n'}, 2,
                    StringSplitOptions.None);
                string prefix;
                if (split[0].Length <= MaxPrefixLength)
                {
                    prefix = split[0];
                }
                else
                {
                    prefix = split[0].Substring(0, MaxPrefixLength - PrefixContinuation.Length) + PrefixContinuation;
                }

                if (controlCharRegex.IsMatch(prefix))
                {
                    prefix = controlCharRegex.Replace(prefix, "");
                }

                return prefix;
            }
            else
            {
                return message;
            }
        }

        private string FormatScopes(object[]? scopes, string? prefix = null, string? separator = null)
        {
            if (scopes != null && scopes.Length > 0)
            {
                StringBuilder builder = new StringBuilder();
                if (prefix != null)
                {
                    builder.Append(prefix);
                }
                for (int i = 0; i < scopes.Length; i++)
                {
                    if (i > 0 && separator != null)
                    {
                        builder.Append(separator);
                    }

                    if (scopes[i] != null)
                    {
                        builder.Append(scopes[i]);
                    }
                }

                return builder.ToString();
            }

            return null;
        }

        private int GetSeverity(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return 2;
                case LogLevel.Error:
                    return 3;
                case LogLevel.Warning:
                    return 4;
                case LogLevel.Information:
                    return 6;
                default:
                    return 7;
            }
        }
    }
}
