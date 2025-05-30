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

        int postIdToUpdate = 5;


        var updatedPost = new PostDto
        {
            Title = "Zaktualizowany tytuł posta!",
            Content = "Zawartość została zmieniona przez PUT."
        };

        var putResponse = await client.PutAsJsonAsync($"{apiUrl}/{postIdToUpdate}", updatedPost);

        if (putResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("Post został zaktualizowany.");
            var putResult = await putResponse.Content.ReadAsStringAsync();
            Console.WriteLine(putResult);
        }
        else
        {
            Console.WriteLine($"Błąd przy aktualizacji: {putResponse.StatusCode}");
            var error = await putResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Szczegóły błędu: " + error);
        }
    }
}