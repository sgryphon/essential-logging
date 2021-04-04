using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EventSource.Library5
{
    public class PrimeGenerator
    {
        private readonly System.Diagnostics.DiagnosticSource _diagnosticSource =
            new DiagnosticListener("DiagnosticSource.Library5");

        private readonly Guid _generatorId = Guid.NewGuid();
        private readonly List<int> _primes = new();

        public IList<int> GeneratePrimes(int n)
        {
            var startingLength = _primes.Count;

            if (PrimerGeneratorEventSource.Log.IsEnabled())
                PrimerGeneratorEventSource.Log.GeneratePrimesStarted(_generatorId, n, startingLength);

            if (n <= 0)
            {
                if (PrimerGeneratorEventSource.Log.IsEnabled())
                    PrimerGeneratorEventSource.Log.EmptyRequest();

                return new int[0];
            }

            int nextPrime;
            if (_primes.Count == 0)
            {
                if (PrimerGeneratorEventSource.Log.IsEnabled())
                {
                    PrimerGeneratorEventSource.Log.InitialiseList(_generatorId);
                    PrimerGeneratorEventSource.Log.FoundPrime(0, 2);
                }
                
                _primes.Add(2);
                nextPrime = 3;
            }
            else
            {
                nextPrime = _primes[^1] + 2;
            }

            while (_primes.Count < startingLength + n)
            {
                var sqrt = (int)Math.Sqrt(nextPrime);
                var isPrime = true;
                for (var i = 0; _primes[i] <= sqrt; i++)
                {
                    if (nextPrime % _primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    if (PrimerGeneratorEventSource.Log.IsEnabled())
                        PrimerGeneratorEventSource.Log.FoundPrime(_primes.Count, nextPrime);

                    _primes.Add(nextPrime);
                }

                nextPrime += 2;
            }

            if (PrimerGeneratorEventSource.Log.IsEnabled())
                PrimerGeneratorEventSource.Log.CopyResult();

            var result = new int[n];
            _primes.CopyTo(startingLength, result, 0, n);

            if (PrimerGeneratorEventSource.Log.IsEnabled())
                PrimerGeneratorEventSource.Log.GeneratePrimesStopping();

            return result;
        }
    }
}
