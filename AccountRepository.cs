using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public sealed class AccountRepository
    {
        private readonly IRepo<int> _idRepo;
        private readonly IRepo<string> _usernameRepo;

        public AccountRepository(IRepo<int> idRepo, IRepo<string> usernameRepo)
        {
            _idRepo = idRepo;
            _usernameRepo = usernameRepo;
        }

        public async Task<EagerAccount> RetrieveAccountAsync()
        {
            var id = await _idRepo.RetrieveAsync();
            var username = await _usernameRepo.RetrieveAsync();

            return new EagerAccount
            (
                id: id,
                username: username
            );
        }

        public LazyAccount RetrieveAsyncAccount()
        {
            return new LazyAccount
            (
                id: new AsyncLazy<int>(() => _idRepo.RetrieveAsync()),
                username: new AsyncLazy<string>(() => _usernameRepo.RetrieveAsync())
            );
        }
    }
}
