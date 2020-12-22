using System;

namespace AsyncBenchmarks
{
    public sealed class LazyAccount
    {
        public LazyAccount(AsyncLazy<int> id, AsyncLazy<string> username, AsyncLazy<string> email, AsyncLazy<DateTime> created, AsyncLazy<bool> active)
        {
            Id = id;
            Username = username;
            Email = email;
            Created = created;
            Active = active;
        }

        public AsyncLazy<int> Id { get; }
        
        public AsyncLazy<string> Username { get; }

        public AsyncLazy<string> Email { get; }

        public AsyncLazy<DateTime> Created { get; }

        public AsyncLazy<bool> Active { get; }
    }
}
