using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class PostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

class Program
{
    static async Task Main()
    {
        string username = "eliza";
        string token = "R6kc/041jp+FbuVB2Ewxm56wMs3RepLrNTRJFbz+B+Q=";
        string apiUrl = "http://localhost:5071/api/postsapi";

        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add("username", username);
        client.DefaultRequestHeaders.Add("token", token);

        var newPost = new PostDto
        {
            Title = "Post z poprawnym typem!",
            Content = "Ten post został przesłany jako poprawnie zdefiniowany obiekt.",
            ImageUrl = null
        };

        var response = await client.PostAsJsonAsync(apiUrl, newPost);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Post został dodany.");
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine($"Błąd: {response.StatusCode}");
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Szczegóły błędu: " + error);
        }
    }
}
