using System;
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
            IRepo<int> idRepo = new SyncRepo<int>(1);
            IRepo<string> usernameRepo = new SyncRepo<string>("name");
            IRepo<string> emailRepo = new SyncRepo<string>("no-reply@ociaw.com");
            IRepo<DateTime> createdRepo = new SyncRepo<DateTime>(DateTime.UnixEpoch);
            IRepo<bool> activeRepo = new SyncRepo<bool>(true);
            _syncRepo = new AccountRepository(idRepo, usernameRepo, emailRepo, createdRepo, activeRepo);
        }
        
        [Benchmark]
        public async Task EagerPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = await _syncRepo.RetrieveAccountAsync();
                _ = account.Id;
                _ = account.Username;
                _ = account.Email;
                _ = account.Created;
                _ = account.Active;
            }
        }

        [Benchmark]
        public async Task LazyPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = _syncRepo.RetrieveAsyncAccount();
                _ = await account.Id;
                _ = await account.Username;
                _ = await account.Email;
                _ = await account.Created;
                _ = await account.Active;
            }
        }

        [Benchmark]
        public async Task EagerPropsReusedAccount()
        {
            var account = await _syncRepo.RetrieveAccountAsync();
            for (int i = 0; i < UsedCallCount; i++)
            {
                _ = account.Id;
                _ = account.Username;
                _ = account.Email;
                _ = account.Created;
                _ = account.Active;
            }
        }

        [Benchmark]
        public async Task LazyPropsReusedAccount()
        {
            var account = _syncRepo.RetrieveAsyncAccount();
            for (int i = 0; i < UsedCallCount; i++)
            {
                _ = await account.Id;
                _ = await account.Username;
                _ = await account.Email;
                _ = await account.Created;
                _ = await account.Active;
            }
        }
    }
}
