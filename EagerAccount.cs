namespace AsyncBenchmarks
{
    public sealed class EagerAccount
    {
        public EagerAccount(int id, string username)
        {
            Id = id;
            Username = username;
        }

        public int Id { get; init; }

        public string Username { get; init; }
    }
}
