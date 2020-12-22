namespace AsyncBenchmarks
{
    public sealed class LazyAccount
    {
        public LazyAccount(AsyncLazy<int> id, AsyncLazy<string> username)
        {
            Id = id;
            Username = username;
        }

        public AsyncLazy<int> Id { get; init; }
        
        public AsyncLazy<string> Username { get; init; }
    }
}
