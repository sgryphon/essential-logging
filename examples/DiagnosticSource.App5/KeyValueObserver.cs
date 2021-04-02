using System;
using System.Collections.Generic;

namespace DiagnosticSource.App5
{
    public class KeyValueObserver : IObserver<KeyValuePair<string, object?>>
    {
        private readonly Action<KeyValuePair<string, object?>> _next;

        public KeyValueObserver(Action<KeyValuePair<string, object?>> next)
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

        public void OnNext(KeyValuePair<string, object?> value)
        {
            _next?.Invoke(value);
        }
    }
}
