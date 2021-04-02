using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiagnosticSource.Library5
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

            var activity = default(Activity);
            if (_diagnosticSource.IsEnabled("GeneratePrimes"))
            {
                activity = new Activity("GeneratePrimes");
                _diagnosticSource.StartActivity(activity,
                    new {GeneratorId = _generatorId, Count = n, StartingLength = startingLength});
            }

            if (n <= 0)
            {
                if (_diagnosticSource.IsEnabled("EmptyRequest"))
                {
                    _diagnosticSource.Write("EmptyRequest", new { });
                }

                return new int[0];
            }

            int nextPrime;
            if (_primes.Count == 0)
            {
                if (_diagnosticSource.IsEnabled("InitialiseList"))
                {
                    _diagnosticSource.Write("InitialiseList", new {GeneratorId = _generatorId});
                }

                _diagnosticSource.Write("FoundPrime",
                    new NextPrimeDiagnostic {Index = _primes.Count, Value = 2});
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
                    if (_diagnosticSource.IsEnabled("FoundPrime"))
                    {
                        _diagnosticSource.Write("FoundPrime",
                            new NextPrimeDiagnostic {Index = _primes.Count, Value = nextPrime});
                    }

                    _primes.Add(nextPrime);
                }

                nextPrime += 2;
            }

            if (_diagnosticSource.IsEnabled("CopyResult"))
            {
                _diagnosticSource.Write("CopyResult", new {ResultLength = n, AllPrimes = _primes});
            }

            var result = new int[n];
            _primes.CopyTo(startingLength, result, 0, n);

            if (_diagnosticSource.IsEnabled("GeneratePrimes"))
            {
                _diagnosticSource.StopActivity(activity!, null);
            }

            return result;
        }
    }
}
