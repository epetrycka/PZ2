using System.ComponentModel.DataAnnotations;

namespace MeetLab.Models
{
    public class UserProfile
    {
        [Key]
        public string NickName { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public string? ProfileImageUrl { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
