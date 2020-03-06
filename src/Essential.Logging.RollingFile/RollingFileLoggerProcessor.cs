using System;
using System.Collections.Concurrent;
using System.Threading;
using Essential.Logging;

namespace Essential.Logging.RollingFile
{
    internal class RollingFileLoggerProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 1024;
        
        private readonly BlockingCollection<string> _messageQueue = new BlockingCollection<string>(_maxQueuedMessages);

        private readonly Thread _outputThread;

        private readonly RollingTextWriter _writer;
 
        public RollingFileLoggerProcessor()
        {
            _writer = new RollingTextWriter();
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true, Name = "RollingFileLoggerProcessor.ProcessLogQueue"
            };
            _outputThread.Start();
        }

        internal RollingFileLoggerOptions Options
        {
            get => _writer.Options;
            set => _writer.Options = value;
        }

        public void EnqueueMessage(string message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException) { }
            }
            
            // Adding is complete, so just log the message
            try
            {
                WriteMessage(message);
            }
            catch (Exception) { }
        }

        private void WriteMessage(string message)
        {
            _writer.WriteLine(message);
        }

        private void ProcessLogQueue()
        {
            try
            {
                foreach (var message in _messageQueue.GetConsumingEnumerable())
                {
                    WriteMessage(message);
                }
            }
            catch
            {
                try
                {
                    _messageQueue.CompleteAdding();
                }
                catch { }
            }
        }

        public void Dispose()
        {
            _messageQueue?.CompleteAdding();
            try
            {
                _outputThread.Join(1500); // with timeout in case writer is locked
            }
            catch (ThreadStateException) { }
            _messageQueue?.Dispose();
            
            _writer?.Flush();
            _writer?.Dispose();
        }
    }
}
