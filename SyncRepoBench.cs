using System.Threading.Tasks;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace AsyncBenchmarks
{
    public class SyncRepoBench
    {
        private const int UniqueCount = 100;
        private const int UsedCallCount = 5;
        private readonly AccountRepository _syncRepo;

        public SyncRepoBench()
        {
            IRepo<int> idRepo = new SyncRepo<int>();
            IRepo<string> usernameRepo = new SyncRepo<string>();
            _syncRepo = new AccountRepository(idRepo, usernameRepo);
        }
        
        [Benchmark]
        public async Task EagerPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = await _syncRepo.RetrieveAccountAsync();
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
                var account = _syncRepo.RetrieveAsyncAccount();
                int id = await account.Id;
                string username = await account.Username;
                Debug.WriteLine($"{id}: {username}");
            }
        }

        [Benchmark]
        public async Task EagerPropsReusedAccount()
        {
            var account = await _syncRepo.RetrieveAccountAsync();
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
            var account = _syncRepo.RetrieveAsyncAccount();
            for (int i = 0; i < UsedCallCount; i++)
            {
                int id = await account.Id;
                string username = await account.Username;
                Debug.WriteLine($"{id}: {username}");
            }
        }
    }
}
