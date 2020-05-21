using Microsoft.Extensions.DependencyInjection;

namespace SWS.ApiServer.Services
{
    public class ServiceResolver
    {
        private static ServiceProvider _provider;
        public static IServiceCollection Services { get; set; }

        public static ServiceProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = Services?.BuildServiceProvider();
                }

                return _provider;
            }
        }

        public static T GetService<T>()
        {
            return Provider.GetService<T>();
        }
    }
}
