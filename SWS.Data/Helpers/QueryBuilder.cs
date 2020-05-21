using System;
using System.Linq;
using System.Reflection;

namespace SWS.Data.Helpers
{
    public static class QueryBuilder
    {
        public static string InsertQuery(string tableName, Type type)
        {
            var propNames = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name).ToList();
            return $"INSERT INTO [dbo].[{tableName}] ({string.Join(',', propNames)}) VALUES ({string.Join(',', propNames.Select(x => $"@{x}"))})";
        }

        public static string ByIdQuery(string tableName, string id)
        {
            return $"SELECT * FROM [dbo].[{tableName}] WHERE Id = '{id}'";
        }

        public static string ByParamsQuery(string tableName, string selectionQuery)
        {
            return $"SELECT * FROM [dbo].[{tableName}] WHERE {selectionQuery}";
        }
    }
}
