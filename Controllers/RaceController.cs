using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;
using TeddySmith.Repository;

namespace TeddySmith.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository raceRepository;

        public RaceController(IRaceRepository raceRepository)
        {
            this.raceRepository = raceRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races=await raceRepository.GetAll();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race=await raceRepository.GetByIdAsync(id);
            if(race != null ) 
            return View(race);
            return NotFound("sorry dude");
        }

        public IActionResult soso()
        {
            Console.WriteLine("i'm here in soso");
            return View();
        }

        public string fefe()
        {
            return "this is fefe";
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }
            raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }
}
