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

        int postIdToDelete = 5;

        var deleteRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{apiUrl}/{postIdToDelete}"),
        };

        deleteRequest.Headers.Add("username", username);
        deleteRequest.Headers.Add("token", token);

        var deleteResponse = await client.SendAsync(deleteRequest);

        if (deleteResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("Post został usunięty.");
            var result = await deleteResponse.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine($"Błąd przy usuwaniu: {deleteResponse.StatusCode}");
            var error = await deleteResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Szczegóły błędu: " + error);
        }
    }
}