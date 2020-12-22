using BenchmarkDotNet.Attributes;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public class AsyncRepoBench
    {
        private const int UniqueCount = 100;
        private const int UsedCallCount = 5;
        private readonly AccountRepository _asyncRepo;

        public AsyncRepoBench()
        {
            IRepo<int> idRepo = new AsyncRepo<int>();
            IRepo<string> usernameRepo = new AsyncRepo<string>();
            _asyncRepo = new AccountRepository(idRepo, usernameRepo);
        }
        
        [Benchmark]
        public async Task EagerPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = await _asyncRepo.RetrieveAccountAsync();
                int id = account.Id;
                string username = account.Username;
                Debug.WriteLine($"{id}: {username}");
            }
        }

        [Benchmark]
        public async Task LazyPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = _asyncRepo.RetrieveAsyncAccount();
                int id = await account.Id;
                string username = await account.Username;
                Debug.WriteLine($"{id}: {username}");
            }
        }

        [Benchmark]
        public async Task EagerPropsReusedAccount()
        {
            var account = await _asyncRepo.RetrieveAccountAsync();
            for (int i = 0; i < UsedCallCount; i++)
            {
                int id = account.Id;
                string username = account.Username;
                Debug.WriteLine($"{id}: {username}");
            }
        }

        [Benchmark]
        public async Task LazyPropsReusedAccount()
        {
            var account = _asyncRepo.RetrieveAsyncAccount();
            for (int i = 0; i < UsedCallCount; i++)
            {
                int id = await account.Id;
                string username = await account.Username;
                Debug.WriteLine($"{id}: {username}");
            }
        }
    }
}
