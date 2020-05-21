using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Insight.Database;
using SWS.Data.Helpers;
using SWS.Models.DbModels;

namespace SWS.Data.Repositories
{
    public abstract class BaseRepository<T> : IDisposable where T : BaseEntity
    {
        public string ConnectionString;

        public abstract string TableName { get; }

        private SqlConnection _connection;

        public SqlConnection Connection => _connection ??= new SqlConnection(ConnectionString);

        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<T> CreateAsync(T model)
        {
            var query = QueryBuilder.InsertQuery(TableName, typeof(T));
            await Connection.ExecuteSqlAsync(QueryBuilder.InsertQuery(TableName, typeof(T)), model);
            return await ByIdAsync(model.Id);
        }

        public async Task<T> ByIdAsync(string id)
        {
            return await Connection.SingleSqlAsync<T>(QueryBuilder.ByIdQuery(TableName, id));
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
