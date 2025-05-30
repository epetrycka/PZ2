public class PostViewModel
{
    public MeetLab.Models.Post Post { get; set; }
    public List<MeetLab.Models.Comment> Comments { get; set; }
    public string NewCommentText { get; set; }
}
