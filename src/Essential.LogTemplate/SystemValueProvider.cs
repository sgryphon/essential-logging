using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Essential
{
    /// <summary>
    /// Implements
    /// </summary>
    /// <returns>A string containing the values formatted using the provided template.</returns>
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
    /// ApplicationName, PrincipalName, WindowsIdentityName.
    /// </para>
    /// <para>
    /// An example template that generates the same output as the ConsoleListener is:
    /// "{Source} {EventType}: {Id} : {Message}".
    /// </para>
    /// </remarks>
    public class SystemValueProvider : ITemplateValueProvider
    {
        private string? _applicationName;

        //private IHttpTraceContext httpTraceContext = new HttpContextCurrentAdapter();
        private int _processId;
        private string? _processName;

        private static readonly Environment.SpecialFolder[] _specialFolders = new[]
        {
            Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolder.ApplicationData,
            Environment.SpecialFolder.LocalApplicationData
        };

        private readonly Dictionary<string, Func<object?>> ValueProviders;

        public SystemValueProvider()
        {
            ValueProviders = new Dictionary<string, Func<object?>>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["DateTime"] = () => LocalDateTimeProvider().ToUniversalTime(),
                ["UtcDateTime"] = () => LocalDateTimeProvider().ToUniversalTime(),
                ["LocalDateTime"] = () => LocalDateTimeProvider(),
                ["ThreadId"] = () => Thread.CurrentThread.ManagedThreadId,
                ["Thread"] = () => (object)Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId,
                ["ActivityId"] = () => Trace.CorrelationManager.ActivityId,
                //["CallStack"] = () => Callstack(),
                ["LogicalOperationStack"] = LogicalOperationStack,
                ["ProcessId"] = () => ProcessId(),
                ["MachineName"] = () => Environment.MachineName,
                ["ProcessName"] = ProcessName,
                ["UserName"] = () => Environment.UserName,
                ["UserDomainName"] = () => Environment.UserDomainName,
                ["CommandLine"] = () => Environment.CommandLine,
                ["Process"] = () => Environment.CommandLine,
                ["ApplicationName"] = ApplicationName,
                ["AppDomain"] = () => AppDomain.CurrentDomain.FriendlyName,
                ["PrincipalName"] = () => Thread.CurrentPrincipal?.Identity?.Name,
                ["BaseDirectory"] = () => AppDomain.CurrentDomain.BaseDirectory,
                ["CurrentDirectory"] = () => Environment.CurrentDirectory

                // // case "WINDOWSIDENTITYNAME":
                // //     value = FormatWindowsIdentityName();
                // //     break;
                // // case "REQUESTURL":
                // //     value = HttpTraceContext.RequestUrl;
                // //     break;
                // // case "REQUESTPATH":
                // //     value = HttpTraceContext.RequestPath;
                // //     break;
                // // case "USERHOSTADDRESS":
                // //     value = HttpTraceContext.UserHostAddress;
                // //     break;
                // // case "APPDATA":
                // //     value = HttpTraceContext.AppDataPath;
                // //     break;
                // // case "LISTENER":
                // //     value = (listener == null) ? "" : listener.Name;
                // //     break;
                // default:
                //     value = "{" + name + "}";
                //     return true;
                //
            };

            foreach (var specialFolder in _specialFolders)
            {
                ValueProviders[specialFolder.ToString()] = () =>
                    Environment.GetFolderPath(specialFolder, Environment.SpecialFolderOption.Create);
            }
        }

        public static Func<DateTimeOffset> LocalDateTimeProvider { get; set; } = () => DateTimeOffset.Now;

        public bool TryGetArgumentValue(string name, out object? value)
        {
            if (ValueProviders.TryGetValue(name, out var provider))
            {
                value = provider();
                return true;
            }

            // other environment variables as {%xxxx%}
            if (name.StartsWith("%") && name.EndsWith("%") && name.Length > 2)
            {
                value = Environment.GetEnvironmentVariable(name.Substring(1, name.Length - 2));
                return true;
            }

            value = null;
            return false;
        }

        // /// <summary>
        // /// Gets or sets the context for ASP.NET web trace information.
        // /// </summary>
        // public IHttpTraceContext HttpTraceContext
        // {
        //     get { return httpTraceContext; }
        //     set { httpTraceContext = value; }
        // }

        private string ApplicationName()
        {
            EnsureApplicationName();
            return _applicationName!;
        }

        private void EnsureApplicationName()
        {
            if (_applicationName == null)
            {
                //applicationName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
                var entryAssembly = Assembly.GetEntryAssembly();

                // if (entryAssembly == null)
                // {
                //     var moduleFileName = new StringBuilder(260);
                //     int size = NativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, moduleFileName, moduleFileName.Capacity);
                //     if (size > 0)
                //     {
                //         applicationName = Path.GetFileNameWithoutExtension(moduleFileName.ToString());
                //         return;
                //     }
                //     //I don't want to raise any error here since I have a graceful result at the end.
                // }

                _applicationName = Path.GetFileNameWithoutExtension(entryAssembly.EscapedCodeBase);
            }
        }

        private void EnsureProcessInfo()
        {
            if (_processName == null)
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    _processId = process.Id;
                    _processName = process.ProcessName;
                }
            }
        }

        private string? LogicalOperationStack()
        {
            Stack? stack = Trace.CorrelationManager.LogicalOperationStack;

            if (stack != null && stack.Count > 0)
            {
                StringBuilder stackBuilder = new StringBuilder();
                foreach (object o in stack)
                {
                    if (stackBuilder.Length > 0) stackBuilder.Append(", ");
                    stackBuilder.Append(o);
                }

                return stackBuilder.ToString();
            }
            else
            {
                return null;
            }
        }

        private int ProcessId()
        {
            EnsureProcessInfo();
            return _processId;
        }

        private string ProcessName()
        {
            EnsureProcessInfo();
            return _processName!;
        }

        // internal static object FormatWindowsIdentityName()
        // {
        //     var identity = WindowsIdentity.GetCurrent();
        //     object value = "";
        //     if (identity != null)
        //     {
        //         value = identity.Name;
        //     }
        //     return value;
        // }
    }
}
