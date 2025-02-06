using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TeddySmith.Data;
using TeddySmith.Models;

namespace TeddySmith.Controllers
{
    public class RaceController : Controller
    {
        private readonly MyDbContext context;

        public RaceController(MyDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<Race> races=context.Races.ToList();
            return View(races);
        }
        public IActionResult Detail(int id)
        {
            Race race=context.Races.Include(a=>a.Address).SingleOrDefault(r => r.Id == id);
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

    }
}
