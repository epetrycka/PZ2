namespace MeetLab.Models.ViewModels
{
    public class UserFriendsViewModel
    {
        public List<MeetLab.Models.User> Friends { get; set; } = new();
        public List<MeetLab.Models.User> SearchedFriends { get; set; } = new();
        public bool WasSearchPerformed { get; set; }
        public List<(Friendship Friendship, User Sender)> PendingFriendRequests { get; set; } = new();
    }
}