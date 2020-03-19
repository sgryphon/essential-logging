using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Essential.IO;
using Essential.Logging.RollingFile;

namespace Essential.Logging
{
    internal class RollingTextWriter : IDisposable
    {
        const int _maxStreamRetries = 5;

        private string _currentPath;
        private TextWriter _currentWriter;
        private object _fileLock = new object();
        private IFileSystem _fileSystem = new FileSystem();
        SystemValueProvider _systemValueProvider = new SystemValueProvider();

        public RollingTextWriter()
        {
        }

        // /// <summary>
        // /// Create RollingTextWriter with filePathTemplate which might contain 1 environment variable in front.
        // /// </summary>
        // /// <param name="filePathTemplate"></param>
        // /// <returns></returns>
        // public static RollingTextWriter Create(string filePathTemplate)
        // {
        //     var segments = filePathTemplate.Split('%');
        //     if (segments.Length > 3)
        //     {
        //         throw new ArgumentException("InitializeData should contain maximum 1 environment variable.", "filePathTemplate");
        //     }
        //     else if (segments.Length == 3)
        //     {
        //         var variableName = segments[1];
        //         var rootFolder = Environment.GetEnvironmentVariable(variableName);
        //         if (String.IsNullOrEmpty(rootFolder))
        //         {
        //             if (variableName.Equals("ProgramData", StringComparison.CurrentCultureIgnoreCase) && (Environment.OSVersion.Version.Major <= 5))//XP or below: https://msdn.microsoft.com/en-us/library/windows/desktop/ms724832%28v=vs.85%29.aspx
        //             {//So the host program could run well in XP and Windows 7 without changing the config file.
        //                 rootFolder = Path.Combine(Environment.GetEnvironmentVariable("AllUsersProfile"), "Application Data");
        //             }
        //             else
        //             {
        //                 throw new ArgumentException("Environment variable is not recognized in InitializeData.", "filePathTemplate");
        //             }
        //         }
        //         var filePath = rootFolder + segments[2];
        //         return new RollingTextWriter(filePath);
        //     }
        //
        //     return new RollingTextWriter(filePathTemplate);
        // }

        internal RollingFileLoggerOptions Options { get; set; }

        public string FilePathTemplate
        {
            get { return Options.FilePathTemplate; }
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
            set
            {
                lock (_fileLock)
                {
                    _fileSystem = value;
                }
            }
        }

        public void Flush()
        {
            lock (_fileLock)
            {
                if (_currentWriter != null)
                {
                    _currentWriter.Flush();
                }
            }
        }

        // public void Write(string value)
        // {
        //     string filePath = GetCurrentFilePath();
        //     lock (_fileLock)
        //     {
        //         EnsureCurrentWriter(filePath);
        //         _currentWriter.Write(value);
        //     }
        // }

        public void WriteLine(string value)
        {
            string filePath = GetCurrentFilePath();
            lock (_fileLock)
            {
                EnsureCurrentWriter(filePath);
                _currentWriter.WriteLine(value);
            }
        }

        private void EnsureCurrentWriter(string path)
        {
            // NOTE: This is called inside lock(_fileLock)
            if (_currentPath != path)
            {
                if (_currentWriter != null)
                {
                    _currentWriter.Close();
                    _currentWriter.Dispose();
                    _currentWriter = null;
                    _currentPath = null;
                }

                var num = 0;
                var stream = default(Stream);

                while (stream == null && num < _maxStreamRetries)
                {
                    var fullPath = num == 0 ? path : getFullPath(path, num);
                    try
                    {
                        stream = FileSystem.Open(fullPath, FileMode.Append, FileAccess.Write, FileShare.Read);

                        this._currentWriter = new StreamWriter(stream);
                        this._currentPath = path;

                        return;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        throw;
                    }
                    catch (IOException)
                    {

                    }
                    num++;
                }

                throw new InvalidOperationException(Resource_RollingFile.RollingTextWriter_ExhaustedLogfileNames);
            }
        }

        static string getFullPath(string path, int num)
        {
            var extension = Path.GetExtension(path);
            return path.Insert(path.Length - extension.Length, "-" + num.ToString(CultureInfo.InvariantCulture));
        }

        private string GetCurrentFilePath()
        {
            var result = StringTemplate.Format(CultureInfo.CurrentCulture, FilePathTemplate, 
                delegate(string name, out object value)
                {
                    if (!_systemValueProvider.TryGetArgumentValue(name, out value))
                    {
                        value = "{" + name + "}";
                    }
                    return true;
                });
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_currentWriter != null)
                {
                    _currentWriter.Dispose();
                }
            }
        }
    }
}
