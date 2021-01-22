using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public class ContinueWithBench
    {
        [Benchmark]
        public async Task<Tuple<int, int, int, int, int>> AwaitYields()
        {
            int i = 0;
            return await Clean(
                i,
                t => Yield(t),
                t => Yield(t),
                t => Yield(t),
                t => Yield(t),
                t => Yield(t)
            );
        }

        [Benchmark]
        public async Task<Tuple<int, int, int, int, int>> ContinueWithYields()
        {
            int i = 0;
            return await Monstrosity(
                i,
                t => Yield(t),
                t => Yield(t),
                t => Yield(t),
                t => Yield(t),
                t => Yield(t)
            );
        }

        [Benchmark]
        public async Task<Tuple<int, int, int, int, int>> AwaitCompleted()
        {
            int i = 0;
            return await Clean(
                i,
                t => Complete(t),
                t => Complete(t),
                t => Complete(t),
                t => Complete(t),
                t => Complete(t)
            );
        }

        [Benchmark]
        public async Task<Tuple<int, int, int, int, int>> ContinueWithCompleted()
        {
            int i = 0;
            return await Monstrosity(
                i,
                t => Complete(t),
                t => Complete(t),
                t => Complete(t),
                t => Complete(t),
                t => Complete(t)
            );
        }

        [Benchmark]
        public async Task<Tuple<int, int, int, int, int>> AwaitDelay()
        {
            int i = 0;
            return await Clean(
                i,
                t => Delay(t),
                t => Delay(t),
                t => Delay(t),
                t => Delay(t),
                t => Delay(t)
            );
        }

        [Benchmark]
        public async Task<Tuple<int, int, int, int, int>> ContinueWithDelay()
        {
            int i = 0;
            return await Monstrosity(
                i,
                t => Delay(t),
                t => Delay(t),
                t => Delay(t),
                t => Delay(t),
                t => Delay(t)
            );
        }

        private static Task<int> Complete(int i)
        {
            return Task.FromResult(i);
        }

        private static async Task<int> Yield(int i)
        {
            await Task.Yield();
            return i;
        }

        private static async Task<int> Delay(int i)
        {
            await Task.Delay(16);
            return i;
        }

        private async static Task<Tuple<TProp1, TProp2, TProp3, TProp4, TProp5>> Clean<TIn, TProp1, TProp2, TProp3, TProp4, TProp5>(TIn input, 
            Func<TIn, Task<TProp1>> selector1Func,
            Func<TIn, Task<TProp2>> selector2Func,
            Func<TIn, Task<TProp3>> selector3Func,
            Func<TIn, Task<TProp4>> selector4Func,
            Func<TIn, Task<TProp5>> selector5Func)
        {
            var item1Task = selector1Func(input);
            var item2Task = selector2Func(input);
            var item3Task = selector3Func(input);
            var item4Task = selector4Func(input);
            var item5Task = selector5Func(input);

            return Tuple.Create(
                await item1Task ?? throw new ArgumentNullException(),
                await item2Task ?? throw new ArgumentNullException(),
                await item3Task ?? throw new ArgumentNullException(),
                await item4Task ?? throw new ArgumentNullException(),
                await item5Task ?? throw new ArgumentNullException()
            );
        }

        private static Task<Tuple<TProp1, TProp2, TProp3, TProp4, TProp5>> Monstrosity<TIn, TProp1, TProp2, TProp3, TProp4, TProp5>(TIn input, 
            Func<TIn, Task<TProp1>> selector1Func,
            Func<TIn, Task<TProp2>> selector2Func,
            Func<TIn, Task<TProp3>> selector3Func,
            Func<TIn, Task<TProp4>> selector4Func,
            Func<TIn, Task<TProp5>> selector5Func)
        {
            Exception? ex1 = null;
            Exception? ex2 = null;
            Exception? ex3 = null;
            Exception? ex4 = null;
            Exception? ex5 = null;

            TProp1 item1 = default;
            TProp2 item2 = default;
            TProp3 item3 = default;
            TProp4 item4 = default;
            TProp5 item5 = default;

            return Task.WhenAll(
                Task.Run(() => selector1Func(input)).ContinueWith(tp => (item1, ex1) = tp.Status == TaskStatus.Faulted ? ((TProp1?)default, tp.Exception) : (tp.Result, (AggregateException?)null)),
                Task.Run(() => selector2Func(input)).ContinueWith(tp => (item2, ex2) = tp.Status == TaskStatus.Faulted ? ((TProp2?)default, tp.Exception) : (tp.Result, (AggregateException?)null)),
                Task.Run(() => selector3Func(input)).ContinueWith(tp => (item3, ex3) = tp.Status == TaskStatus.Faulted ? ((TProp3?)default, tp.Exception) : (tp.Result, (AggregateException?)null)),
                Task.Run(() => selector4Func(input)).ContinueWith(tp => (item4, ex4) = tp.Status == TaskStatus.Faulted ? ((TProp4?)default, tp.Exception) : (tp.Result, (AggregateException?)null)),
                Task.Run(() => selector5Func(input)).ContinueWith(tp => (item5, ex5) = tp.Status == TaskStatus.Faulted ? ((TProp5?)default, tp.Exception) : (tp.Result, (AggregateException?)null))
            )
            .ContinueWith(tpa => 
            {
                if (ex1 is not null)
                    throw ex1;
                if (ex2 is not null)
                    throw ex2;
                if (ex3 is not null)
                    throw ex3;
                if (ex4 is not null)
                    throw ex4;
                if (ex5 is not null)
                    throw ex5;

                return new Tuple<TProp1, TProp2, TProp3, TProp4, TProp5>(
                    item1 ?? throw new ArgumentNullException(), 
                    item2 ?? throw new ArgumentNullException(),
                    item3 ?? throw new ArgumentNullException(),
                    item4 ?? throw new ArgumentNullException(),
                    item5 ?? throw new ArgumentNullException()
                );
            });
        }
    }
}
