namespace MeetLab.Models;

public class Message
{
    public int Id { get; set; }
    public string SenderNick { get; set; }
    public string ReceiverNick { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsRead { get; set; }
}
