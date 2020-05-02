using System.ServiceProcess;

namespace SWS.TokenGeneratorService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new HttpListenerService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
