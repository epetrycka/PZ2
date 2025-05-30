using System.ComponentModel.DataAnnotations;

namespace MeetLab.Models;

public class Post
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public string AuthorNick { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Comment> Comments { get; set; } = new();
}
