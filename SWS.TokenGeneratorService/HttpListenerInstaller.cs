using System.ComponentModel;
using System.ServiceProcess;

namespace SWS.TokenGeneratorService
{
    [RunInstaller(true)]
    public partial class HttpListenerInstaller : System.Configuration.Install.Installer
    {
        public HttpListenerInstaller()
        {
            InitializeComponent();

            var serviceInstaller = new ServiceInstaller();
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.NetworkService
            };

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "SWT Token Generator Service";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
