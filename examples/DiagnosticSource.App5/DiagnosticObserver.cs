using System;
using System.Diagnostics;

namespace DiagnosticSource.App5
{
    public class DiagnosticObserver : IObserver<DiagnosticListener>
    {
        private readonly Action<DiagnosticListener> _next;

        public DiagnosticObserver(Action<DiagnosticListener> next)
        {
            _next = next;
        }
        
        public void OnCompleted()
        {
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public void OnNext(DiagnosticListener value) => _next?.Invoke(value);
    }
}
