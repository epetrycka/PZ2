public class MessageViewModel
{
    public string WithUser { get; set; }
    public string Box { get; set; } // "inbox" lub "sent"
    public List<MeetLab.Models.Message> Messages { get; set; }
}
