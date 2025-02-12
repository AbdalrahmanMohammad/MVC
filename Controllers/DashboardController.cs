using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TeddySmith.Interfaces;
using TeddySmith.Models;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository dashboardRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPhotoService photoService;
        private readonly IUserRepository userRepository;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService, IUserRepository userRepository)
        {
            this.dashboardRepository = dashboardRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.photoService = photoService;
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await dashboardRepository.GetAllUserRaces();
            var userClubs = await dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs,
            };
            return View(dashboardViewModel);
        }
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = httpContextAccessor.HttpContext.User.GetUserId();
            var user = await userRepository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                AddressId = user.AddressId,
                Address = user.Address
            };
            return View(editUserViewModel);
        }
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult img)
        {
            user.Id = editVM.Id;
            user.ProfileImageUrl = img?.Url?.ToString() ?? user.ProfileImageUrl;
            user.AddressId = editVM.AddressId;
            user.Address = new Address
            {
                Street = editVM.Address.Street,
                City = editVM.Address.City,
                State = editVM.Address.State,
            };
            user.Pace=editVM.Pace;  
            user.Mileage = editVM.Mileage;  
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to pass the validation");
                return View("EditUserProfile", editVM);
            }
            var user = await userRepository.GetUserById(editVM.Id);
            ImageUploadResult photoResult = null;

            if (editVM.Image != null)
            {
                if (user.ProfileImageUrl != null)
                    try
                    {
                        string publicId = photoService.GetPublicIdFromUrl(user.ProfileImageUrl);
                        await photoService.DeletePhotoAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "could not delete photo");
                        return View(editVM);
                    }
                photoResult = await photoService.AddPhotoAsync(editVM.Image);
            }
            MapUserEdit(user, editVM, photoResult);
            userRepository.Update(user);
            return RedirectToAction("Index");

        }
    }
}
