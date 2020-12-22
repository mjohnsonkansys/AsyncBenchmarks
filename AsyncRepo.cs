using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public sealed class AsyncRepo<T> : IRepo<T>
    {
        public T? Value { get; set; }

        public int DelayMs { get; set; }

        public async Task<T?> RetrieveAsync()
        {
            if (DelayMs > 0)
                await Task.Delay(DelayMs);
            else
                await Task.Yield();

            return Value;
        }
    }
}
