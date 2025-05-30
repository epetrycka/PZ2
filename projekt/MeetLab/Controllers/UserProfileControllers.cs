using Microsoft.AspNetCore.Mvc;
using MeetLab.Data;
using MeetLab.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetLab.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly AppDbContext _context;

        public UserProfileController(AppDbContext context)
        {
            _context = context;
        }

        private string GetCurrentUserNick()
        {
            return HttpContext.Session.GetString("user") ?? "UnknownUser";
        }

        public IActionResult Index()
        {
            var nick = GetCurrentUserNick();
            var profile = _context.UserProfiles.FirstOrDefault(p => p.NickName == nick);
            return View(profile);
        }

        public IActionResult Edit()
        {
            var nick = GetCurrentUserNick();
            var profile = _context.UserProfiles.FirstOrDefault(p => p.NickName == nick) ?? new UserProfile { NickName = nick };
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserProfile model, IFormFile image)
        {
            var nick = GetCurrentUserNick();
            var existing = _context.UserProfiles.FirstOrDefault(p => p.NickName == nick);

            string? imagePath = null;

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                imagePath = "/images/" + fileName;

            }
            else
            {
                imagePath = null;
            }

            // if (image != null && image.Length > 0)
            // {
            //     var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            //     Directory.CreateDirectory(uploadsDir);

            //     var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            //     var filePath = Path.Combine(uploadsDir, fileName);

            //     using (var stream = new FileStream(filePath, FileMode.Create))
            //     {
            //         await image.CopyToAsync(stream);
            //     }

            //     imagePath = "/uploads/" + fileName;
            // }

            if (existing == null)
            {
                model.NickName = nick;
                model.UpdatedAt = DateTime.Now;
                if (imagePath != null)
                    model.ProfileImageUrl = imagePath;
                _context.UserProfiles.Add(model);
            }
            else
            {
                existing.Description = model.Description;
                existing.Status = model.Status;
                if (imagePath != null)
                    existing.ProfileImageUrl = imagePath;
                existing.UpdatedAt = DateTime.Now;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete()
        {
            var nick = GetCurrentUserNick();
            var profile = _context.UserProfiles.FirstOrDefault(p => p.NickName == nick);
            if (profile != null)
            {
                _context.UserProfiles.Remove(profile);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
