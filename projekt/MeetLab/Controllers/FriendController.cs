using Microsoft.AspNetCore.Mvc;
using MeetLab.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetLab.Controllers;

[Route("Friend")]
public class FriendController : Controller
{
    private readonly MeetLab.Data.AppDbContext _context;

    public FriendController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Friends")]
    public IActionResult Friends()
    {
        var currentUserNick = HttpContext.Session.GetString("user");
        var currentUser = _context.Users
            .Include(u => u.Friends)
            .FirstOrDefault(u => u.NickName == currentUserNick);

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var friends = currentUser.Friends.ToList();

        var pendingFriendships = _context.Friendship
            .Where(f => f.Receiver == currentUserNick && f.Status == Status.SENT)
            .Join(_context.Users,
                f => f.Sender,
                u => u.NickName,
                (f, u) => new { Friendship = f, Sender = u })
            .ToList()
            .Select(p => (p.Friendship, p.Sender))
            .ToList();

        var viewModel = new MeetLab.Models.ViewModels.UserFriendsViewModel
        {
            Friends = friends,
            PendingFriendRequests = pendingFriendships
        };

        return View(viewModel);
    }

    [HttpPost("SearchFriend")]
    public IActionResult SearchFriend(IFormCollection form)
    {
        string nickname = form["nickname"].ToString();

        var searchedUsers = _context.Users
            .Where(u => u.NickName.Contains(nickname))
            .ToList();

        var currentUserNick = HttpContext.Session.GetString("user");
        var currentUser = _context.Users
            .Include(u => u.Friends)
            .FirstOrDefault(u => u.NickName == currentUserNick);

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var friends = currentUser.Friends.ToList();

        var viewModel = new MeetLab.Models.ViewModels.UserFriendsViewModel
        {
            Friends = friends,
            SearchedFriends = searchedUsers,
            WasSearchPerformed = true,
        };

        return View("Friends", viewModel);
    }

    [HttpGet("detail/{nickname}")]
    public IActionResult FriendDetail(string nickname)
    {
        var currentUserNick = HttpContext.Session.GetString("user");
        if (string.IsNullOrEmpty(currentUserNick)) return RedirectToAction("Login", "Auth");

        var friend = _context.Users.FirstOrDefault(u => u.NickName == nickname);
        if (friend == null) return NotFound();

        var existingFriendship = _context.Friendship
            .FirstOrDefault(f =>
                (f.Sender == currentUserNick && f.Receiver == nickname) ||
                (f.Sender == nickname && f.Receiver == currentUserNick)
            );

        bool areFriends = false;

        areFriends = _context
            .Users
            .Where(u => u.NickName == currentUserNick)
            .SelectMany(u => u.Friends)
            .Any(f => f.NickName == nickname);

        var userProfile = _context.UserProfiles.FirstOrDefault(u => u.NickName == nickname) ?? null;

        var model = new MeetLab.Models.ViewModels.FriendDetailViewModel
        {
            Friend = friend,
            CurrentUserNick = currentUserNick,
            ExistingFriendship = existingFriendship,
            UserProfile = userProfile,
            AreFriends = areFriends
        };

        return View(model);
    }

    [HttpPost("send")]
    public IActionResult SendFriendRequest(string receiver)
    {
        var currentUserNick = HttpContext.Session.GetString("user");

        var existingFriendship = _context.Friendship
            .FirstOrDefault(f => 
                (f.Sender == currentUserNick && f.Receiver == receiver) ||
                (f.Sender == receiver && f.Receiver == currentUserNick));

        if (existingFriendship != null)
        {
            if (existingFriendship.Status == Status.DENIED)
            {
                existingFriendship.Status = Status.SENT;
                existingFriendship.ModificationDate = DateTime.Now;
                _context.SaveChanges();
            }
        }
        else
        {
            var friendship = new Friendship
            {
                Sender = currentUserNick,
                Receiver = receiver,
                Status = Status.SENT,
                ModificationDate = DateTime.Now
            };
            _context.Friendship.Add(friendship);
            _context.SaveChanges();
        }

        return RedirectToAction("FriendDetail", new { nickname = receiver });
    }

    [HttpPost("accept")]
    public IActionResult AcceptFriend(int id)
    {
        var friendship = _context.Friendship.Find(id);
        if (friendship != null)
        {
            friendship.Status = Status.ACCEPTED;
            friendship.ModificationDate = DateTime.Now;

            var sender = _context.Users.FirstOrDefault(u => u.NickName == friendship.Sender);
            var receiver = _context.Users.FirstOrDefault(u => u.NickName == friendship.Receiver);

            if (sender != null && receiver != null)
            {
                if (!sender.Friends.Contains(receiver))
                    sender.Friends.Add(receiver);

                if (!receiver.Friends.Contains(sender))
                    receiver.Friends.Add(sender);
            }
            _context.SaveChanges();
        }
        return RedirectToAction("FriendDetail", new { nickname = friendship?.Sender });
    }

   [HttpPost]
    public IActionResult DenyFriend(int id)
    {
        var friendship = _context.Friendship.FirstOrDefault(f => f.Id == id);
        if (friendship != null)
        {
            friendship.Status = Status.DENIED;
            friendship.ModificationDate = DateTime.Now;
            _context.SaveChanges();
        }

        return RedirectToAction("FriendDetail", new { nickname = friendship?.Receiver == HttpContext.Session.GetString("user") ? friendship.Sender : friendship.Receiver });
    }


    [HttpPost("remove")]
    public IActionResult RemoveFriend(int id)
    {
        var friendship = _context.Friendship.Find(id);
        if (friendship != null)
        {
            var sender = _context.Users.Include(u => u.Friends).FirstOrDefault(u => u.NickName == friendship.Sender);
            var receiver = _context.Users.Include(u => u.Friends).FirstOrDefault(u => u.NickName == friendship.Receiver);

            if (sender != null && receiver != null)
            {
                sender.Friends.Remove(receiver);
                receiver.Friends.Remove(sender);
            }

            _context.Friendship.Remove(friendship);

            _context.SaveChanges();
        }

        return RedirectToAction("Friends");
    }
}