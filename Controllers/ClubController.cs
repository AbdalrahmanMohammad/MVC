using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Models;

namespace TeddySmith.Controllers
{
    public class ClubController : Controller
    {
        private readonly MyDbContext context;

        public ClubController(MyDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<Club> clubs = context.Clubs.ToList();
            return View(clubs);
        }

        public IActionResult Detail(int id)
        {
            Club club = context.Clubs.Include(a => a.Address).SingleOrDefault(r => r.Id == id);
            if (club != null)
                return View(club);
            return NotFound("sorry dude");
        }
    }
}
