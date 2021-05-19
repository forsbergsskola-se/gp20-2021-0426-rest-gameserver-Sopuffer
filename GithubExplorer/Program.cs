using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
namespace GithubExplorer
{
    class Program
    {

        public static string userName;
        public static string token;
        static HttpClient client = new HttpClient();
        static HttpContent content;
        static HttpResponseMessage response;
        static GitHubUser user;
        static JsonSerializerOptions option;
        static string result;
        static async Task Main(string[] args)
        {
            Task t = new Task(HttpGetUser);
            while (token == null)
            {
                Console.WriteLine("Welcome to Sopuffer's Github! Please write Sopuffer as usename: ");
                userName = Console.ReadLine();

                if (userName != "sopuffer")
                {

                    Console.WriteLine("You cannot enter this user. Please try again: ");
                    continue;
                }
                Console.WriteLine("Great! Now Enter authorization token");
                token = Console.ReadLine();
                
                t.Start();
            }
            GithubOptions();   
            Console.ReadLine();
        }

        static async void HttpGetUser()
        {

            if (userName != null)
            {
                var URL = "https://api.github.com/users/" + userName;

                Console.WriteLine("GET: " + URL + "\n");

                client.DefaultRequestHeaders.UserAgent.Add(
                new System.Net.Http.Headers.ProductInfoHeaderValue("GitHubExplorer", "0.1"));
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                response = await client.GetAsync(URL);
                content = response.Content;
                result = await content.ReadAsStringAsync();

                if (result != null)
                {
                    option = new JsonSerializerOptions();
                    option.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    user = JsonSerializer.Deserialize<GitHubUser>(result, option);

                    Console.WriteLine(user + "\n");

                    var r = client.GetStringAsync($"{user.Repos_url}").Result;
                    var l = JsonSerializer.Deserialize<List<Repo>>(r, option);

                    Console.WriteLine("Current Repositories: \n");
                    foreach (var repo in l)
                    {
                        Console.WriteLine("* " + repo.Name);
                    }
                    Console.WriteLine("\n\n");
                    Console.WriteLine("Please choose between number 0 & 3 to check out the repositories: ");

                }

            }

        }
        static void GithubOptions()
        {
            bool correctChoice = false;
            while (!correctChoice)
            {
                var input = Console.ReadLine();
                int value;

                if (int.TryParse(input, out value))
                {
                    int repositoryNumber = Convert.ToInt32(input);
                    if (repositoryNumber < 0 || repositoryNumber > 3)
                    {
                        Console.WriteLine("This number is out of range. Please try again: \n");
                        continue;
                    }
                    else
                    {
                        ChooseRepository(repositoryNumber);
                        correctChoice = true;
                    }
                }
                else
                {
                    Console.WriteLine("This is not a number. Please try again: \n");
                    continue;
                }

            }
        }
        static void ChooseRepository(int repositoryNumber)
        {

            bool hasChosenAnAction = false;
            option = new JsonSerializerOptions();
            option.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            var r = client.GetStringAsync($"{user.Repos_url}").Result;
            var l = JsonSerializer.Deserialize<List<Repo>>(r, option);
            Repo repo = l[repositoryNumber];

            Console.WriteLine("Repository Name: " + repo.Name);
            if (repo.Description != null)
            {
                Console.WriteLine("Repository description: " + repo.Description);
            }
            else
            {
                Console.WriteLine("Description: No description available");
            }
            Console.WriteLine("Repository html: " + repo.Html_url);
            Console.WriteLine("Repository url: " + repo.Url);
            Console.WriteLine("\n");


            Console.WriteLine("Press e to exit this repository");
            Console.WriteLine("Want to read the comments or add a comment? Press c.");
            Console.WriteLine("Want to read the issues or add an issue? Press i.");


            while (!hasChosenAnAction)
            {
                var response = Console.ReadLine();
                int value;
                if (int.TryParse(response, out value))
                {
                    Console.WriteLine("This is a number. Its not valid. Please try again.");
                    continue;
                }
                else
                {
                    if(response!= "e" && response!="c" && response!= "i")
                    {
                        Console.WriteLine("This is an invalid answer. Please try again.");
                        continue;
                    }
                    else
                    {
                        switch (response)
                        {
                            case "e":
                                Console.WriteLine("Please choose between number 0 & 3 to check out the repositories: ");
                                GithubOptions();
                                break;
                            case "i":
                                CreateIssue(repositoryNumber);
                                break;
                            case "c":
                                CreateComment(repositoryNumber);
                                break;
                        }
                        hasChosenAnAction = true;
                    }
                }
            }
        }

        static void CreateIssue(int repositoryNumber)
        {
            var r = client.GetStringAsync($"{user.Repos_url}").Result;
            var l = JsonSerializer.Deserialize<List<Repo>>(r, option);
            Repo repo = l[repositoryNumber];

            foreach (var issueInfo in l)
            {
                var index = issueInfo.Issues_url.IndexOf("{");
                issueInfo.Issues_url = issueInfo.Issues_url.Remove(index);
            }
            Console.WriteLine("\n\n");

            var repoInfo = client.GetStringAsync(l[repositoryNumber].Issues_url).Result;
            var issues = JsonSerializer.Deserialize<List<Issue>>(repoInfo, option);

            Console.WriteLine("Current issues in " + l[1].Issues_url);
            foreach (var issue in issues)
            {
                Console.WriteLine($"Title: {issue.title}\r\nInfo: {issue.body} \n");
            }

            Console.WriteLine("\n\n");
            var newIssue = new Issue();
            Console.WriteLine("Start by entering the Title to your issue: ");
            string Title = Console.ReadLine();
            newIssue.title = Title;
            Console.WriteLine("Enter the Body to your issue:");
            string Body = Console.ReadLine();
            newIssue.body = Body;
            var response = client.PostAsJsonAsync(repo.Issues_url, newIssue).Result;
            Console.WriteLine("\n\n");
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Title: " + newIssue.title);
                Console.WriteLine("Info: " + newIssue.body);
            }
            Console.WriteLine("Please choose between number 0 & 3 to check out the repositories: ");

            GithubOptions();
        }

        static void CreateComment(int repositoryNumber)
        {
            option = new JsonSerializerOptions();
            option.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            var r = client.GetStringAsync($"{user.Repos_url}").Result;
            var l = JsonSerializer.Deserialize<List<Repo>>(r, option);
            Repo repo = l[repositoryNumber];
            Console.WriteLine("Enter your comment: ");
            var body = Console.ReadLine();
            var newComment = new Comment();
            newComment.Body = body;

            var response = client.PostAsJsonAsync(repo.Comments_url, newComment).Result;
            Console.WriteLine("\n\n");
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        /*Source: Github.com/forsbergsskola-se/gp20-2021-0426-rest-gameserver-kevinlempa*/
        public class Issue
        {
            public string title { get; set; }
            public string body { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string comments_url { get; set; }
        }

        public class Comment
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public Uri Url { get; set; }
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
            public string Comments_url { get; set; }

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

