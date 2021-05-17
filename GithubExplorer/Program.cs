using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
namespace GithubExplorer
{
    class Program
    {

        public static string userName;

        static async Task Main(string[] args)
        {
            Task t = new Task(HttpGetUser);
            Console.WriteLine("Welcome to Github! Please enter a Github User: ");
            userName = Console.ReadLine();
            if (userName != null)
            {
                t.Start();
            }
            Console.ReadLine();
        }

        static async void HttpGetUser()
        {

            if (userName != null)
            {
                var URL = "https://api.github.com/users/" + userName;

                HttpClientHandler handler = new HttpClientHandler();
                Console.WriteLine("GET: " + URL + "\n");
                HttpClient client = new HttpClient(handler);
                var byteArray = Encoding.ASCII.GetBytes("username:password1234" + "\n");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                HttpResponseMessage response = await client.GetAsync(URL);
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                if (result != null)
                {
                    var option = new JsonSerializerOptions();
                    option.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    var user = JsonSerializer.Deserialize<GitHubUser>(result, option);

                    Console.WriteLine(user + "\n");

                    var r = client.GetStringAsync($"{user.Repos_url}").Result;
                    var l = JsonSerializer.Deserialize<List<Repo>>(r, option);

                    Console.WriteLine("Current Repositories: \n");
                    foreach (var repo in l)
                    {
                        Console.WriteLine( "* "+ repo.Name);
                    }

                    foreach(var repo in l)
                    {
                        var index = repo.Issues_url.IndexOf("{");
                        repo.Issues_url = repo.Issues_url.Remove(index);
                        Console.WriteLine(repo.Issues_url);
                    }

                    Console.WriteLine(l[0].Issues_url);
                    var repoInfo1 = client.GetStringAsync(l[0].Issues_url).Result;
                    var issues = JsonSerializer.Deserialize<List<Issue>>(repoInfo1, option);

                    foreach (var issue in issues)
                    {
                        Console.WriteLine($"Title : {issue.Title}\r\nInfo :{issue.Body}");
                    }

                    //var commentJSon = client.GetStringAsync(issues[1].Comments_url).Result;
                    //var comments = JsonSerializer.Deserialize<List<Issue>>(commentJSon, option);
                    //Console.WriteLine(commentJSon);
                    //foreach (var issue in comments)
                    //{
                    //    Console.WriteLine($"Title : {issue.Title}\r\nInfo :{issue.Body}\r\n");
                    //}

                }

            }
        }
        /*Source: Github.com/forsbergsskola-se/gp20-2021-0426-rest-gameserver-kevinlempa*/
        public class Issue
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public DateTime Created_at { get; set; }
            public DateTime Updated_at { get; set; }
            public string Comments_url { get; set; }
        }
        public class Repo
        {
            public string Name { get; set; }
            public string Html_url { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }
            public string Issues_url { get; set; }
        }
        public class GitHubUser
        {
            public string Login { get; set; }
            public string Repos_url { get; set; }
            public string Followers_url { get; set; }
            public string Organinzations_url { get; set; }
            public string Company { get; set; }
            public string Blog { get; set; }
            public string Location { get; set; }
            public string Email { get; set; }
            public string Hireable { get; set; }
            public string Bio { get; set; }
            public int Public_repos { get; set; }
            public int Followers { get; set; }
            public int Following { get; set; }
            public string Created_at { get; set; }
            public string Updated_at { get; set; }

            string PrintToConsole()
            {
                if (string.IsNullOrEmpty(Company))
                {
                    Company = "Not available";
                }

                if (string.IsNullOrEmpty(Location))
                {
                    Location = "Not available";
                }

                if (string.IsNullOrEmpty(Email))
                {
                    Email = "Not available";
                }

                if (string.IsNullOrEmpty(Blog))
                {
                    Blog = "Not available";
                }

                if (string.IsNullOrEmpty(Hireable))
                {
                    Hireable = "Unknown";
                }

                if (string.IsNullOrEmpty(Bio))
                {
                    Bio = "Not available";
                }

                return $"User : {Login}\r\n" +
                       $"Location : {Location}\r\n" +
                       $"Email : {Email}\r\n" +
                       $"Hireable : {Hireable}\r\n" +
                       $"Blog : {Blog}\r\n" +
                       $"Bio : {Bio}\r\n" +
                       $"Public Repos : {Public_repos}\r\n" +
                       $"Followers : {Followers}\r\n" +
                       $"Following : {Following}\r\n" +
                       $"Created at : {Created_at}\r\n" +
                       $"Updated at : {Updated_at}";
            }

            public override string ToString()
            {
                return PrintToConsole();
            }
        }
    }
}

