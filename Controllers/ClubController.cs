using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository clubRepository;
        private readonly IPhotoService photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            this.clubRepository = clubRepository;
            this.photoService = photoService;
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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address {
                        Street=clubVM.Address.Street,
                        City=clubVM.Address.City,  
                        State=clubVM.Address.State,
                    },
                    ClubCategory = clubVM.ClubCategory,
                };
                clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else { ModelState.AddModelError("", "photo upload failed"); }
            return View(clubVM);
        }
    }
}
