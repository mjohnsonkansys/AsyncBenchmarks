using BenchmarkDotNet.Running;

namespace AsyncBenchmarks
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
