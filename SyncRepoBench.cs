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
        private readonly AccountRepository _repo;

        public SyncRepoBench()
        {
            IRepo<int> idRepo = new SyncRepo<int>(1);
            IRepo<string> usernameRepo = new SyncRepo<string>("name");
            IRepo<string> emailRepo = new SyncRepo<string>("no-reply@ociaw.com");
            IRepo<DateTime> createdRepo = new SyncRepo<DateTime>(DateTime.UnixEpoch);
            IRepo<bool> activeRepo = new SyncRepo<bool>(true);
            _repo = new AccountRepository(idRepo, usernameRepo, emailRepo, createdRepo, activeRepo);
        }
        
        [Benchmark]
        public async Task AllEagerPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = await _repo.RetrieveAccountAsync();
                _ = account.Id;
                _ = account.Username;
                _ = account.Email;
                _ = account.Created;
                _ = account.Active;
            }
        }

        [Benchmark]
        public async Task AllLazyPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = _repo.RetrieveAsyncAccount();
                _ = await account.Id;
                _ = await account.Username;
                _ = await account.Email;
                _ = await account.Created;
                _ = await account.Active;
            }
        }
        
        [Benchmark]
        public async Task TwoEagerPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = await _repo.RetrieveAccountAsync();
                _ = account.Id;
                _ = account.Username;
            }
        }

        [Benchmark]
        public async Task TwoLazyPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = _repo.RetrieveAsyncAccount();
                _ = await account.Id;
                _ = await account.Username;
            }
        }

        [Benchmark]
        public async Task AllEagerPropsReusedAccount()
        {
            var account = await _repo.RetrieveAccountAsync();
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
        public async Task AllLazyPropsReusedAccount()
        {
            var account = _repo.RetrieveAsyncAccount();
            for (int i = 0; i < UsedCallCount; i++)
            {
                _ = await account.Id;
                _ = await account.Username;
                _ = await account.Email;
                _ = await account.Created;
                _ = await account.Active;
            }
        }

        [Benchmark]
        public async Task TwoEagerPropsReusedAccount()
        {
            var account = await _repo.RetrieveAccountAsync();
            for (int i = 0; i < UsedCallCount; i++)
            {
                _ = account.Id;
                _ = account.Username;
            }
        }

        [Benchmark]
        public async Task TwoLazyPropsReusedAccount()
        {
            var account = _repo.RetrieveAsyncAccount();
            for (int i = 0; i < UsedCallCount; i++)
            {
                _ = await account.Id;
                _ = await account.Username;
            }
        }
    }
}
