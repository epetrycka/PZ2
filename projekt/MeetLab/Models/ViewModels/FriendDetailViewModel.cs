namespace MeetLab.Models.ViewModels;

public class FriendDetailViewModel
{
    public User Friend { get; set; }
    public string CurrentUserNick { get; set; }
    public Friendship? ExistingFriendship { get; set; }
}
