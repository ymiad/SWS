using System.Threading.Tasks;
using Insight.Database;
using SWS.Data.Helpers;
using SWS.Models.DbModels;

namespace SWS.Data.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public override string TableName => "User";

        public UserRepository(string connectionString) : base(connectionString) { }

        public async Task<User> ByUsername(string username)
        {
            var selectionQuery = $"Username = '{username}'";
            var user = await Connection.SingleSqlAsync<User>(QueryBuilder.ByParamsQuery(TableName, selectionQuery));
            return user;
        }
    }
}
