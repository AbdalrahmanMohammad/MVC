using Microsoft.AspNetCore.Mvc;
using TeddySmith.Interfaces;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class DashboardController: Controller
    {
        private readonly IDashboardRepository dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }
        public async Task<IActionResult> Index()
        { 
            var userRaces=await dashboardRepository.GetAllUserRaces();
            var userClubs=await dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs,
            };
            return View(dashboardViewModel);
        }
    }
}
