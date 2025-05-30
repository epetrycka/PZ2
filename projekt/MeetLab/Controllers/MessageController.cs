using Microsoft.AspNetCore.Mvc;
using MeetLab.Models;

namespace MeetLab.Controllers;

[Route("Message")]
public class MessageController : Controller
{
    private readonly MeetLab.Data.AppDbContext _context;
    public MessageController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Index")]
    public IActionResult Index()
    {
        var currentUser = HttpContext.Session.GetString("user");
        var friends = _context.Friendship
            .Where(f => (f.Sender == currentUser || f.Receiver == currentUser) && f.Status == Status.ACCEPTED)
            .Select(f => f.Sender == currentUser ? f.Receiver : f.Sender)
            .Distinct()
            .ToList();
        return View(friends);
    }

    [HttpGet("Conversation/{withUser}")]
    public IActionResult Conversation(string withUser, string box = "inbox")
    {
        var currentUser = HttpContext.Session.GetString("user");

        List<Message> messages;

        if (box == "inbox")
        {
            messages = _context.Messages
                .Where(m => m.ReceiverNick == currentUser && m.SenderNick == withUser)
                .OrderByDescending(m => m.SentDate)
                .ToList();
        }
        else
        {
            messages = _context.Messages
                .Where(m => m.SenderNick == currentUser && m.ReceiverNick == withUser)
                .OrderByDescending(m => m.SentDate)
                .ToList();
        }

        var vm = new MessageViewModel
        {
            WithUser = withUser,
            Box = box,
            Messages = messages
        };

        return View(vm);
    }

    [HttpPost("SendMessage")]
    public IActionResult SendMessage(string receiverNick, string title, string content)
    {
        var currentUser = HttpContext.Session.GetString("user");

        var message = new Message
        {
            SenderNick = currentUser,
            ReceiverNick = receiverNick,
            Title = title,
            Content = content,
            SentDate = DateTime.Now,
            IsRead = false
        };

        _context.Messages.Add(message);
        _context.SaveChanges();

        return RedirectToAction("Conversation", new { withUser = receiverNick, box = "sent" });
    }
}
