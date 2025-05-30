using System.Net.Http.Headers;
using System.Net.Http.Json;

string username = "eliza";
string token = "R6kc/041jp+FbuVB2Ewxm56wMs3RepLrNTRJFbz+B+Q=";

string apiUrl = "http://localhost:5071/api/postsapi";

using var client = new HttpClient();

client.DefaultRequestHeaders.Add("username", username);
client.DefaultRequestHeaders.Add("token", token);

try
{
    var response = await client.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode)
    {
        var posts = await response.Content.ReadFromJsonAsync<List<PostDto>>();

        Console.WriteLine("Pobrano posty:");
        foreach (var post in posts!)
        {
            Console.WriteLine($"[{post.Id}] {post.Title} - {post.CreatedAt} (autor: {post.AuthorNick})");
            Console.WriteLine(post.Content);
            Console.WriteLine(new string('-', 40));
        }
    }
    else
    {
        Console.WriteLine($"Błąd: {response.StatusCode}");
        string details = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Szczegóły: " + details);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Wystąpił błąd:");
    Console.WriteLine(ex.Message);
}

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AuthorNick { get; set; } = default!;
}
