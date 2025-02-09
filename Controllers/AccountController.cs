using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeddySmith.Data;
using TeddySmith.Models;
using TeddySmith.ViewModels;

namespace TeddySmith.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;  
        private readonly MyDbContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, MyDbContext context)
        {
            _context = context;
            _userManager = userManager; 
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user != null)
            {//user found
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded) { return RedirectToAction("Index", "Race"); }
                }
                TempData["Error"] = "wrong credantials. Please try again dude";
                return View(loginViewModel);
            }
            TempData["Error"] = "user not even found dude";
            return View(loginViewModel);
        }
    }
}
