using System;
using SWS.Data.Repositories;

namespace SWS.Data
{
    public class MainDbFacade : IDisposable
    {
        public string ConnectionString { get; set; }

        public MainDbFacade(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private UserRepository _userRepository;

        public UserRepository UserRepository => _userRepository ??= new UserRepository(ConnectionString);

        public void Dispose()
        {
            _userRepository?.Dispose();
        }
    }
}
