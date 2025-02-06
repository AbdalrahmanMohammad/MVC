using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;

namespace TeddySmith.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository clubRepository;

        public ClubController(IClubRepository clubRepository)
        {
            this.clubRepository = clubRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await clubRepository.GetByIdAsync(id);
            if (club != null)
                return View(club);
            return NotFound("sorry dude");
        }
    }
}
