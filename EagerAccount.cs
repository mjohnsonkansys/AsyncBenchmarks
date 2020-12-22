using System;

namespace AsyncBenchmarks
{
    public sealed class EagerAccount
    {
        public EagerAccount(int id, string username, string email, DateTime created, bool active)
        {
            Id = id;
            Username = username;
            Email = email;
            Created = created;
            Active = active;
        }

        public int Id { get; }

        public string Username { get; }

        public string Email { get; }

        public DateTime Created { get; }

        public bool Active { get; }
    }
}
