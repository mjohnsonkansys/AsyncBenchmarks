using BenchmarkDotNet.Attributes;
using System;
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
            IRepo<int> idRepo = new AsyncRepo<int>(1);
            IRepo<string> usernameRepo = new AsyncRepo<string>("name");
            IRepo<string> emailRepo = new AsyncRepo<string>("no-reply@ociaw.com");
            IRepo<DateTime> createdRepo = new AsyncRepo<DateTime>(DateTime.UnixEpoch);
            IRepo<bool> activeRepo = new AsyncRepo<bool>(true);
            _asyncRepo = new AccountRepository(idRepo, usernameRepo, emailRepo, createdRepo, activeRepo);
        }
        
        [Benchmark]
        public async Task EagerPropsUniqueAccount()
        {
            for (int i = 0; i < UniqueCount; i++)
            {
                var account = await _asyncRepo.RetrieveAccountAsync();
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
                var account = _asyncRepo.RetrieveAsyncAccount();
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
            var account = await _asyncRepo.RetrieveAccountAsync();
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
            var account = _asyncRepo.RetrieveAsyncAccount();
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
