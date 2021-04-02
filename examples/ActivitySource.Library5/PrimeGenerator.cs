using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ActivitySource.Library5
{
    public class PrimeGenerator
    {
        private readonly System.Diagnostics.ActivitySource _activitySource =
            new("ActivitySource.Library5");

        private readonly Guid _generatorId = Guid.NewGuid();
        private readonly List<int> _primes = new();

        public IList<int> GeneratePrimes(int n)
        {
            var activity = default(Activity);
            try
            {
                if (_activitySource.HasListeners())
                {
                    activity = _activitySource.StartActivity("GeneratePrimes", ActivityKind.Internal,
                        Activity.Current?.Context ?? new ActivityContext(),
                        new ActivityTagsCollection {["GeneratorId"] = _generatorId, ["Count"] = n});
                }

                var startingLength = _primes.Count;
                activity?.AddTag("StartingLength", startingLength);

                if (n <= 0)
                {
                    activity?.AddEvent(new ActivityEvent("EmptyRequest"));

                    return new int[0];
                }

                int nextPrime;
                if (_primes.Count == 0)
                {
                    if (activity != null)
                    {
                        activity.AddEvent(new ActivityEvent("InitialiseList"));
                        activity.AddEvent(new ActivityEvent("FoundPrime",
                            tags: new ActivityTagsCollection {["Index"] = _primes.Count, ["Value"] = 2}));
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
                        activity?.AddEvent(new ActivityEvent("FoundPrime",
                            tags: new ActivityTagsCollection {["Index"] = _primes.Count, ["Value"] = 2}));

                        _primes.Add(nextPrime);
                    }

                    nextPrime += 2;
                }

                activity?.AddEvent(new ActivityEvent("CopyResult"));

                var result = new int[n];
                _primes.CopyTo(startingLength, result, 0, n);

                return result;
            }
            finally
            {
                activity?.Dispose();
            }
        }
    }
}
