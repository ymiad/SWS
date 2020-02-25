using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWS.CardWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var id = args.FirstOrDefault(x => x.Contains("id:") && x.Length > 3)?.Split(':')[1];

            if (string.IsNullOrEmpty(id)) return;

            var client = new HttpClient();

            var guidRequest = await client.GetAsync("http://localhost:8888/");
            var token = await guidRequest.Content.ReadAsStringAsync();

            var json = "{\"id\": \"" + id + "\", \"token\": \"" + token + "\"}";

            var value = new Dictionary<string, string>
            {
                {"token", HashString(json)}
            };

            var content = new FormUrlEncodedContent(value);

            await client.PostAsync("http://localhost:8888/", content);

            Console.ReadKey();
        }

        public static string HashString(string value)
        {
            using (var hash = SHA256.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(value))
                    .Select(item => item.ToString("x2")));
            }
        }
    }
}
