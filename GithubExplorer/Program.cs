using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace GithubExplorer
{
    class Program
    {
        public static string input;
        static async Task Main(string[] args)
        {
            Task t = new Task(HTTP_GET);
            Console.WriteLine("Welcome to Github! Please enter a Github User: ");
            input = Console.ReadLine();
            if (input != null){
                t.Start();
            }
            Console.ReadLine();
        }

        static async void HTTP_GET()
        {
           
            if (input != null) {
                var URL = "https://api.github.com/users/" + input;
                HttpClientHandler handler = new HttpClientHandler();
                Console.WriteLine("GET: " + URL);
                HttpClient client = new HttpClient(handler);
                var byteArray = Encoding.ASCII.GetBytes("username:password1234");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                HttpResponseMessage response = await client.GetAsync(URL);
                HttpContent content = response.Content;
                Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);
                string result = await content.ReadAsStringAsync();
                if (result != null) {
                    Console.WriteLine(result);
                }
            }

        }
    }
}

