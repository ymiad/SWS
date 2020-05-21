using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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
            StartHttpListener();
        }

        private async Task StartHttpListener()
        {
            _listener.Prefixes.Add("http://localhost:8888/");
            _listener.Start();
            _guid = Guid.NewGuid().ToString();

            while (true)
            {
                if (string.IsNullOrEmpty(_guid))
                {
                    _guid = Guid.NewGuid().ToString();
                }

                var request = _listener.GetContext().Request;

                if (request.QueryString.AllKeys.Contains("id"))
                {
                    await SendCardId(request.QueryString["id"]);
                }
                else
                {
                    var response = _listener.GetContext().Response;
                    var buffer = Encoding.UTF8.GetBytes(_guid);
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }

        private async Task SendCardId(string cardId)
        {
            var client = new HttpClient();
            var data = "{\"id\": \"" + cardId + "\", \"token\": \"" + _guid + "\"}";
            var value = new Dictionary<string, string> {{"token", HashString(data)}};
            var content = new FormUrlEncodedContent(value);
            var url = $"{Environment.GetEnvironmentVariable("SWSServerUrl")}/Card/AttachCard";

            System.Diagnostics.Process.Start("CMD.exe", $"{data} {url}");

            await client.PostAsync(url, content);
        }

        private static string HashString(string value)
        {
            using (var hash = SHA256.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(value))
                    .Select(item => item.ToString("x2")));
            }
        }

        protected override void OnStop()
        {
            _listener.Stop();
        }
    }
}
