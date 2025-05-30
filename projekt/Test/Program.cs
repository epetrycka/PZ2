using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeetLabApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var username = "admin";
            var token = "your_token_here";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Username", username);
            client.DefaultRequestHeaders.Add("Token", token);

            var newPost = new
            {
                Title = "Sample Post",
                Content = "This is a sample post content.",
                ImageUrl = "http://example.com/image.jpg"
            };

            var json = JsonConvert.SerializeObject(newPost);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5000/api/posts", content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Response: {responseString}");
        }
    }
}
