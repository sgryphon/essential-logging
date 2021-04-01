using System;
using System.Collections.Generic;

namespace DiagnosticSource.Library5
{
    public class PrimeGenerator
    {
        private readonly List<int> _primes = new();

        public IList<int> GeneratePrimes(int n)
        {
            if (n <= 0)
            {
                return new int[0];
            }

            var startingLength = _primes.Count;

            int nextPrime;
            if (_primes.Count == 0)
            {
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
                    _primes.Add(nextPrime);
                }

                nextPrime += 2;
            }

            var result = new int[n];
            _primes.CopyTo(startingLength, result, 0, n);

            return result;
        }
    }
}
