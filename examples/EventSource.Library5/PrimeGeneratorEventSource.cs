﻿using System;
using System.Diagnostics.Tracing;
using System.Security.Cryptography;

namespace EventSource.Library5
{
    [EventSource(Name="EventSource-Library-PrimeGenerator")]
    public sealed class PrimerGeneratorEventSource : System.Diagnostics.Tracing.EventSource
    {
        [Event(2901, Keywords = EventKeywords.None, Task=Task.GeneratePrimes, Opcode=EventOpcode.Start, Level=EventLevel.Informational, ActivityOptions = EventActivityOptions.None)]
        public void GeneratePrimesStarted(Guid generatorId, int count, int startingLength) { WriteEvent(2901, generatorId, count, startingLength);}

        [Event(2902, Keywords = EventKeywords.None, Task=Task.GeneratePrimes, Opcode=EventOpcode.Stop, Level=EventLevel.Informational, ActivityOptions = EventActivityOptions.None)]
        public void GeneratePrimesStopping() { WriteEvent(2902); }
        
        [Event(2903, Keywords = Keywords.Generator|Keywords.Overhead, Level=EventLevel.Verbose)]
        public void EmptyRequest() { WriteEvent(2903);}
        
        [Event(2904, Keywords = Keywords.Generator|Keywords.Overhead, Level=EventLevel.Verbose)]
        public void InitialiseList(Guid generatorId) { WriteEvent(2904, generatorId);}
        
        [Event(2905, Keywords = Keywords.Generator, Level=EventLevel.Verbose)]
        public void FoundPrime(int index, int value) { WriteEvent(2905, index, value);}
        
        [Event(2906, Keywords = Keywords.Generator|Keywords.Overhead, Level=EventLevel.Verbose)]
        public void CopyResult(int resultLength) { WriteEvent(2906, resultLength);}
        
        public static PrimerGeneratorEventSource Log = new PrimerGeneratorEventSource();

        public static class Keywords // FlagsEnum
        {
            public const EventKeywords Generator = (EventKeywords)0x0001;
            public const EventKeywords Overhead = (EventKeywords)0x0002;
        }

        public static class Task
        {
            public const EventTask GeneratePrimes = (EventTask)1;
        }
    }
}
