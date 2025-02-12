using Microsoft.AspNetCore.Mvc;
using TeddySmith.Interfaces;
using TeddySmith.Models;
using TeddySmith.Repository;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var list = await userRepository.GetAllUsers();
            List<UserViewModel> users = new List<UserViewModel>();

            foreach (var i in list)
            {
                users.Add(new UserViewModel
                {
                    Id = i.Id,
                    UserName = i.UserName,
                    Pace = i.Pace,
                    Mileage = i.Mileage
                });
            }

            return View(users);
        }
        public async Task<IActionResult> Detail(string id)
        {
            var user = await userRepository.GetUserById(id);
            var userToReturn = new UserDetailViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };
            if (user != null)
                return View(userToReturn);
            return NotFound("sorry dude");
        }
    }
}
