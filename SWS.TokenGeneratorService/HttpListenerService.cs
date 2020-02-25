using System;
using System.Net;
using System.ServiceProcess;

namespace SWS.TokenGeneratorService
{
    public partial class HttpListenerService : ServiceBase
    {
        private readonly HttpListener _listener;
        private string _guid;

        public HttpListenerService()
        {
            InitializeComponent();
            _listener = new HttpListener();
        }

        protected override void OnStart(string[] args)
        {
            _listener.Prefixes.Add("http://localhost:8888/");
            _listener.Start();
            _guid = Guid.NewGuid().ToString();

            while (true)
            {
                var context = _listener.GetContext();
                var response = context.Response;

                if (string.IsNullOrEmpty(_guid))
                {
                    _guid = Guid.NewGuid().ToString();
                }

                var buffer = System.Text.Encoding.UTF8.GetBytes(_guid);
                response.ContentLength64 = buffer.Length;
                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        protected override void OnStop()
        {
            _listener.Stop();
        }
    }
}
