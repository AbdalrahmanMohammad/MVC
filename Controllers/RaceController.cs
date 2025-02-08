using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;
using TeddySmith.Repository;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository raceRepository;
        private readonly IPhotoService photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            this.raceRepository = raceRepository;
            this.photoService = photoService;
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
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    RaceCategory = raceVM.RaceCategory,
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    },
                    Image = result.Url.ToString(),
                };
                raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else { ModelState.AddModelError("", "photo upload failed"); }
            return View(raceVM);
        }
                public async Task<IActionResult> Edit(int id)
        {
            var race = await raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory,
            };
            return View(raceVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "photo upload failed");
                return View("Edit", raceVM); 
            }
            var userRace = await raceRepository.GetByIdAsyncNoTracking(id);
            if (userRace == null)
            {
                return View("Error");
            }

            ImageUploadResult photoResult = null;

            if (raceVM.Image != null)
            {
                if (userRace.Image != null)
                    try
                    {
                        string publicId = photoService.GetPublicIdFromUrl(userRace.Image);
                        await photoService.DeletePhotoAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "could not delete photo");
                        return View(raceVM);
                    }
                photoResult = await photoService.AddPhotoAsync(raceVM.Image);
            }

            var race = new Race
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = photoResult?.Url?.ToString() ?? userRace.Image, 
                AddressId = userRace.AddressId,
                Address = raceVM.Address,
            };
            raceRepository.Update(race);
            return RedirectToAction("Index");
        }
    }
}
