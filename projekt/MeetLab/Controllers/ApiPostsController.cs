using Microsoft.AspNetCore.Mvc;
using MeetLab.Data;
using MeetLab.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetLab.ApiControllers
{
    [ApiController]
    [Route("/api[controler]")]
    public class PostsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/posts
        [HttpGet]
        public IActionResult GetAll()
        {
            var posts = _context.Posts.Include(p => p.Comments).ToList();
            return Ok(posts);
        }

        // GET: api/posts/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        // POST: api/posts
        [HttpPost]
        public IActionResult Create([FromBody] Post post)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null) return Unauthorized();

            post.AuthorNick = user.NickName;
            post.CreatedAt = DateTime.Now;
            _context.Posts.Add(post);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }

        // PUT: api/posts/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Post updatedPost)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null) return Unauthorized();

            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();
            if (post.AuthorNick != user.NickName) return Forbid();

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            post.ImageUrl = updatedPost.ImageUrl;
            _context.SaveChanges();

            return Ok(post);
        }

        // DELETE: api/posts/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null) return Unauthorized();

            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();
            if (post.AuthorNick != user.NickName) return Forbid();

            _context.Posts.Remove(post);
            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/posts/{postId}/comments
        [HttpPost("{postId}/comments")]
        public IActionResult AddComment(int postId, [FromBody] Comment comment)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null) return Unauthorized();

            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null) return NotFound();

            comment.PostId = postId;
            comment.AuthorNick = user.NickName;
            comment.CreatedAt = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = postId }, comment);
        }
    }
}