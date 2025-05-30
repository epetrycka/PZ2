using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetLab.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Post")]
    public int PostId { get; set; }

    [Required]
    public string AuthorNick { get; set; }

    [Required]
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public Post Post { get; set; }
}
