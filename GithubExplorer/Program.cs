using System;
using System.Net.Sockets;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading;

namespace GithubExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            GithubServer.Start();
            Console.ReadLine();
        }
    }

    class GithubServer
    {
        static readonly HttpClient client = new HttpClient();
        public static async void Start()
        {
            Console.WriteLine("Please add a github Username to explore");
            string input = Console.ReadLine();

            if (input != null)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://www.github.com/"+input);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }

                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }
}
