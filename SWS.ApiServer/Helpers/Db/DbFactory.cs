using Microsoft.Extensions.Options;
using SWS.Data;

namespace SWS.ApiServer.Helpers.Db
{
    public class DbFactory
    {
        private readonly IOptions<AppSettings> _config;

        public DbFactory(IOptions<AppSettings> config)
        {
            _config = config;
        }

        public MainDbFacade CreateMainDb()
        {
            return new MainDbFacade(_config.Value.DbConnectionString);
        }
    }
}
