using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public interface IRepo<T>
    {
        public T? Value { get; set; }

        public Task<T?> RetrieveAsync();
    }
}
