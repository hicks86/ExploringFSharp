using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using LanguageExt;
using Microsoft.FSharp.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Benchmarking
{

    //Run in Release!!!!

    [SimpleJob(RunStrategy.Monitoring)]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class FunctionalListBencmarks
    {
        [Params(1_000, 10_000, 100_000, 1_000_000, 10_000_000)]
        public int ListCountNumber;

        private Lst<int> languageExtLst;
        private ImmutableList<int> sysImmutable;
        private FSharpList<int> fsharpList;
        private FSharpList<int> fsharpInCSharpList;

        [GlobalSetup]
        public void Setup()
        {
            var range = Enumerable.Range(1, ListCountNumber);

            languageExtLst = range.Freeze();
            sysImmutable = range.ToImmutableList();
            fsharpList = range.ToFsharpList();
            fsharpInCSharpList = range.ToFsharpList();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            languageExtLst = Lst<int>.Empty;
            sysImmutable = ImmutableList<int>.Empty;
            fsharpList = FSharpList<int>.Empty;
            fsharpInCSharpList = FSharpList<int>.Empty;
        }

        [Benchmark(Baseline = true)]
        public void SystemImmutable()
        {
            var sum = 0;
            var index = 0;

            var result = sysImmutable.Aggregate((sum: 0, index: 0), (acc, item) =>
                (acc.sum + acc.index + item, acc.index + 1));

            sum = result.sum;
            index = result.index;
        }

        [Benchmark]
        public void LanguageExt()
        {
            var sum = 0;
            var index = 0;

            var result = languageExtLst.Aggregate((sum: 0, index: 0), (acc, item) =>
                (acc.sum + acc.index + item, acc.index + 1));

            sum = result.sum;
            index = result.index;
        }

        [Benchmark]
        public void FSharp()
        {
            var sum = 0;
            var index = 0;

            var result = global::FSharpLoopSumFunction.RunLoop(fsharpList);

            sum = result.Item1;
            index = result.Item2;
        }

        [Benchmark]
        public void FSharpInCSharp()
        {
            var sum = 0;
            var index = 0;

            var result = fsharpInCSharpList.Aggregate((sum: 0, index: 0), (acc, item) =>
                (acc.sum + acc.index + item, acc.index + 1));

            sum = result.sum;
            index = result.index;
        }
    }

    public static class Extensions
    {
        public static FSharpList<T> ToFsharpList<T>(this IEnumerable<T> value) => ListModule.OfSeq(value);
    }
}