using Microsoft.AspNetCore.Mvc;
using MeetLab.Models;

namespace MvcMovie.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsApiController : ControllerBase
{
    private readonly MeetLab.Data.AppDbContext _context;

    public PostsApiController(MeetLab.Data.AppDbContext context)
    {
        _context = context;
    }

    public class PostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    public class CreatePostResponse
    {
        public string Message { get; set; } = string.Empty;
        public int PostId { get; set; }
    }

    [HttpPost]
    public IActionResult CreatePost([FromBody] PostDto dto, [FromHeader(Name = "username")] string username, [FromHeader(Name = "token")] string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == username && u.Token == token);
        if (user == null)
        {
            return Unauthorized("Niepoprawna autoryzacja.");
        }

        if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
        {
            return BadRequest("Tytuł i treść są wymagane.");
        }

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            AuthorNick = username,
            CreatedAt = DateTime.Now,
            ImageUrl = dto.ImageUrl
        };

        _context.Posts.Add(post);
        _context.SaveChanges();

        var response = new CreatePostResponse
        {
            Message = "Post został dodany.",
            PostId = post.Id
        };

        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetPosts([FromHeader(Name = "username")] string username, [FromHeader(Name = "token")] string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == username && u.Token == token);
        if (user == null)
        {
            return Unauthorized("Niepoprawna autoryzacja.");
        }

        var posts = _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return Ok(posts);
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePost(int id, [FromBody] PostDto dto, [FromHeader(Name = "username")] string username, [FromHeader(Name = "token")] string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == username && u.Token == token);
        if (user == null)
        {
            return Unauthorized("Niepoprawna autoryzacja.");
        }

        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound("Post nie istnieje.");
        }

        if (post.AuthorNick != username)
        {
            return Forbid("Nie możesz edytować posta innego użytkownika.");
        }

        if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
        {
            return BadRequest("Tytuł i treść są wymagane.");
        }

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.ImageUrl = dto.ImageUrl;

        _context.SaveChanges();

        return Ok(new { message = "Post został zaktualizowany.", postId = post.Id });
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeletePost(int id, [FromHeader(Name = "username")] string username, [FromHeader(Name = "token")] string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.NickName == username && u.Token == token);
        if (user == null)
        {
            return Unauthorized("Niepoprawna autoryzacja.");
        }

        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound("Post nie istnieje.");
        }

        if (post.AuthorNick != username)
        {
            return Forbid("Nie możesz usunąć posta innego użytkownika.");
        }

        _context.Posts.Remove(post);
        _context.SaveChanges();

        return Ok(new { message = "Post został usunięty.", postId = id });
    }


}
