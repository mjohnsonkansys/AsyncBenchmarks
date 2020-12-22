using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public class AsyncLazyBench
    {
        private const int IterationCount = 100;

        [Benchmark]
        public async Task<int> AsyncLazySingle()
        {
            var lazy = new AsyncLazy<int>(() => AsyncCall());
            return await lazy;
        }

        [Benchmark]
        public async Task<int> TaskSingle()
        {
            return await AsyncCall();
        }

        [Benchmark]
        public async Task<int> AsyncLazyMulti()
        {
            var lazy = new AsyncLazy<int>(() => AsyncCall());
            for (int i = 0; i < IterationCount; i++)
                await lazy;
            return await lazy;
        }

        [Benchmark]
        public async Task<int> TaskMulti()
        {
            var lazy = new AsyncLazy<int>(() => AsyncCall());
            for (int i = 0; i < IterationCount; i++)
                await AsyncCall();
            return await AsyncCall();
        }

        private async Task<int> AsyncCall()
        {
            await Task.Yield();
            return 42;
        }
    }
}
