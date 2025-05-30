using System.ComponentModel.DataAnnotations;

namespace MeetLab.Models;

public class User
{
    [Key]
    [Required]
    [Display(Name = "NickName")]
    public required String NickName { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public required String FirstName { get; set; }

    [Required]
    [Display(Name = "Password")]
    public required String Password { get; set; }

    [Display(Name = "Unique authentication token")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public String? Token { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Registration date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime RegistrationDate { get; set; }

    [Display(Name = "Friends list")]
    public List<User> Friends { get; set; } = new();
}