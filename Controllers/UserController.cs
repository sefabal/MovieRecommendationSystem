using Microsoft.AspNetCore.Mvc;
using MovieRecommender.Models;
using MovieRecommender.Services;
using System;
using System.Threading.Tasks;

namespace MovieRecommender.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        // GET: /<controller>/

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {

            if (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password))
            {
                bool isValid = await userService.CheckUser(user.Username, user.Password);
                if (isValid)
                {
                    var currentUSer = await userService.GetUserByName(user.Username);

                    if (HttpContext.Session.Get<User>(SessionExtensions.UserKey) == default(User))
                    {
                        currentUSer.Rates = new System.Collections.Generic.List<Rate>();
                        HttpContext.Session.Set<User>(SessionExtensions.UserKey, currentUSer);
                    }
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
