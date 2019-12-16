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

        private readonly IMovieServices movieServices;

        public UserController(IUserService userService, IMovieServices movieServices)
        {
            this.userService = userService;
            this.movieServices = movieServices;
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
                        currentUSer.Password = "";
                        currentUSer.Rates = new System.Collections.Generic.List<Rate>();
                        HttpContext.Session.Set<User>(SessionExtensions.UserKey, currentUSer);
                    }
                }
            }

            return Redirect("/Home/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password))
            {
                var userCheck = await userService.GetUserByName(user.Username);

                if (userCheck == null)
                {
                    await userService.AddUser(user);
                }
                else
                {
                    return Redirect("Register");
                }
            }

            return View("Login", user);
        }


        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var currentUser = HttpContext.Session.Get<User>(SessionExtensions.UserKey);

            if (currentUser != default(User))
            {
                HttpContext.Session.Set<User>(SessionExtensions.UserKey, null);
            }

            return View("Login", new User());
        }

        public async Task<IActionResult> RatedMovies()
        {
            var currentUser = HttpContext.Session.Get<User>(SessionExtensions.UserKey);

            if (currentUser != default(User))
            {
                var user = await userService.GetUserByName(currentUser.Username);

                foreach (Rate rate in user.Rates)
                {
                    rate.Movie = await movieServices.GetMovieById(rate.MovieId);
                }

                return View(user.Rates);
            }

            return View("Login", new User());
        }
    }
}
