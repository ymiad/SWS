using System;
using System.Linq;
using System.Threading.Tasks;
using SWS.ApiServer.Helpers;
using SWS.ApiServer.Helpers.Db;
using SWS.Models.DbModels;

namespace SWS.ApiServer.Services
{
    public class UserService
    {
        public User GetById(string id)
        {
            return new User();
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var dbFactory = ServiceResolver.GetService<DbFactory>();
            using var db = dbFactory.CreateMainDb();

            var user = await db.UserRepository.ByUsername(username);

            if (user == null) return null;

            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new AppException("Password is required");

            // TODO: if exists
            //throw new AppException($"Username \"{user.Username}\" is already taken");

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var dbFactory = ServiceResolver.GetService<DbFactory>();
            using var db = dbFactory.CreateMainDb();

            return await db.UserRepository.CreateAsync(user);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected)", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected)", "passwordSalt");

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != storedHash[i]).Any();
        }
    }
}
