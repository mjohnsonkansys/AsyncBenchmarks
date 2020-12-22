using BenchmarkDotNet.Attributes;
using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public class CallBench
    {
        [Benchmark]
        public async Task<int> AsyncCall() => await Async();

        [Benchmark]
        public async Task<int> MixedCall() => await Sync();
        
        [Benchmark]
        public Task<int> FullSyncCall() => Sync();

        private async Task<int> Async() => 42;

        private Task<int> Sync() => Task.FromResult(42);
    }
}
