﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;
using TeddySmith.Services;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository clubRepository;
        private readonly IPhotoService photoService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            this.clubRepository = clubRepository;
            this.photoService = photoService;
            this.httpContextAccessor = httpContextAccessor;
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
            var curUser = httpContextAccessor.HttpContext.User;
            CreateClubViewModel createClubViewModel;
            if (User.Identity.IsAuthenticated)
                createClubViewModel = new CreateClubViewModel() { AppUserId = curUser.GetUserId() };
            else
                createClubViewModel = new CreateClubViewModel();
            return View(createClubViewModel);
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
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    },
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = clubVM.AppUserId
                };
                clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else { ModelState.AddModelError("", "photo upload failed"); }
            return View(clubVM);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var club = await clubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
                AppUserId=club.AppUserId
            };
            return View(clubVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "photo upload failed");
                return View("Edit", clubVM);
            }
            var userClub = await clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub == null)
            {
                return View("Error");
            }

            ImageUploadResult photoResult = null;

            if (clubVM.Image != null)
            {
                if (userClub.Image != null)
                    try
                    {
                        string publicId = photoService.GetPublicIdFromUrl(userClub.Image);
                        await photoService.DeletePhotoAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "could not delete photo");
                        return View(clubVM);
                    }
                photoResult = await photoService.AddPhotoAsync(clubVM.Image);
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = photoResult?.Url?.ToString() ?? userClub.Image,
                AddressId = userClub.AddressId,
                Address = clubVM.Address,
                AppUserId = clubVM.AppUserId?? null
            };
            clubRepository.Update(club);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var club = await clubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
            };
            return View(clubVM);
        }
        [HttpPost, ActionName("Deletee")]
        public async Task<IActionResult> Delete(int id, EditClubViewModel clubVM)
        {
            var userClub = await clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub == null)
            {
                return View("Error");
            }
            if (userClub.Image != null)
                try
                {
                    string publicId = photoService.GetPublicIdFromUrl(userClub.Image);
                    await photoService.DeletePhotoAsync(publicId);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(clubVM);
                }
            clubRepository.Delete(userClub);
            return RedirectToAction("Index");
        }
    }
}
