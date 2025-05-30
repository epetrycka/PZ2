using Microsoft.AspNetCore.Mvc;
using MeetLab.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MeetLab.Controllers;

[Route("Posts")]
public class PostsController : Controller
{
    private readonly MeetLab.Data.AppDbContext _context;

    public PostsController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var currentUserNick = HttpContext.Session.GetString("user");
        var currentUser = _context.Users
            .Include(u => u.Friends)
            .FirstOrDefault(u => u.NickName == currentUserNick);

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var friendsNicknames = currentUser.Friends?.Select(f => f.NickName).ToList() ?? new List<string>();
        friendsNicknames.Add(currentUserNick);

        var posts = _context.Posts
            .Where(p => friendsNicknames.Contains(p.AuthorNick))
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return View(posts);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    public IActionResult Create(Post post, IFormFile image)
    {
        post.AuthorNick = HttpContext.Session.GetString("user");
        post.CreatedAt = DateTime.Now;

        if (image != null && image.Length > 0)
        {
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            post.ImageUrl = "/images/" + fileName;
        }
        else
        {
            post.ImageUrl = null;
        }

        _context.Posts.Add(post);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost("AddComment")]
    public IActionResult AddComment(int postId, string text)
    {
        var comment = new Comment
        {
            PostId = postId,
            Text = text,
            AuthorNick = HttpContext.Session.GetString("user"),
            CreatedAt = DateTime.Now
        };

        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
