using System.ComponentModel.DataAnnotations;

public class Login
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string LoginName { get; set; }

    [Required]
    public string Password { get; set; }
}