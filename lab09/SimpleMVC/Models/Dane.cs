using System.ComponentModel.DataAnnotations;

public class Dane
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Tekst { get; set; }
}