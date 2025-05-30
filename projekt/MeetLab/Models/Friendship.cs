using System.ComponentModel.DataAnnotations;

namespace MeetLab.Models;

public enum Status
{
    SENT,
    ACCEPTED,
    DENIED
}

public class Friendship
{
    [Key]
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Sender")]
    public required String Sender { get; set; }

    [Required]
    [Display(Name = "Receiver")]
    public required String Receiver { get; set; }

    [Required]
    [Display(Name = "Status")]
    public Status Status { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Last modification date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ModificationDate { get; set; }
}