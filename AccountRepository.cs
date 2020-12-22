using System;
using System.Threading.Tasks;

namespace AsyncBenchmarks
{
    public sealed class AccountRepository
    {
        private readonly IRepo<int> _idRepo;
        private readonly IRepo<string> _usernameRepo;
        private readonly IRepo<string> _emailRepo;
        private readonly IRepo<DateTime> _createdRepo;
        private readonly IRepo<bool> _activeRepo;

        public AccountRepository(
            IRepo<int> idRepo,
            IRepo<string> usernameRepo,
            IRepo<string> emailRepo,
            IRepo<DateTime> createdRepo,
            IRepo<bool> activeRepo
        )
        {
            _idRepo = idRepo;
            _usernameRepo = usernameRepo;
            _emailRepo = emailRepo;
            _createdRepo = createdRepo;
            _activeRepo = activeRepo;
        }

        public async Task<EagerAccount> RetrieveAccountAsync()
        {
            var id = await _idRepo.RetrieveAsync();
            var username = await _usernameRepo.RetrieveAsync();
            var email = await _emailRepo.RetrieveAsync();
            var created = await _createdRepo.RetrieveAsync();
            var active = await _activeRepo.RetrieveAsync();

            return new EagerAccount
            (
                id: id,
                username: username,
                email: email,
                created: created,
                active: active
            );
        }

        public LazyAccount RetrieveAsyncAccount()
        {
            return new LazyAccount
            (
                id: new AsyncLazy<int>(() => _idRepo.RetrieveAsync()),
                username: new AsyncLazy<string>(() => _usernameRepo.RetrieveAsync()),
                email: new AsyncLazy<string>(() => _emailRepo.RetrieveAsync()),
                created: new AsyncLazy<DateTime>(() => _createdRepo.RetrieveAsync()),
                active: new AsyncLazy<bool>(() => _activeRepo.RetrieveAsync())
            );
        }
    }
}
