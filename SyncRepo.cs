using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public sealed class SyncRepo<T> : IRepo<T>
    {
        public T? Value { get; set; }

        public Task<T?> RetrieveAsync() => Task.FromResult(Value);
    }
}
