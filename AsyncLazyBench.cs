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
        public async Task<int> YieldingAsyncLazySingle()
        {
            var lazy = new AsyncLazy<int>(() => YieldingAsyncCall());
            return await lazy;
        }

        [Benchmark]
        public async Task<int> YieldingTaskSingle()
        {
            return await YieldingAsyncCall();
        }

        [Benchmark]
        public async Task<int> YieldingAsyncLazyMulti()
        {
            var lazy = new AsyncLazy<int>(() => YieldingAsyncCall());
            for (int i = 0; i < IterationCount; i++)
                await lazy;
            return await lazy;
        }

        private async Task<int> YieldingAsyncCall()
        {
            await Task.Yield();
            return 42;
        }

        private async Task<int> AsyncCall()
        {
            return 42;
        }
    }
}
