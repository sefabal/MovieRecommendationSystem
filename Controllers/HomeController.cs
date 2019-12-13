using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRecommender.Models;
using MovieRecommender.Models.ViewModels;
using MovieRecommender.Services;

namespace MovieRecommender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMovieServices movieServices;

        private readonly IRatingService ratingService;

        private readonly IUserService userService;

        private readonly MovieContext movieContext;

        public HomeController(ILogger<HomeController> logger, IMovieServices movieService, IRatingService ratingService, IUserService userService,MovieContext movieContext)
        {
            _logger = logger;
            this.movieServices = movieService;
            this.ratingService = ratingService;
            this.userService = userService;
            this.movieContext = movieContext;
        }

        public async Task<IActionResult> Index(int? p = 1, int? s = 12)
        {
            var paginationVal = s.GetValueOrDefault();
            var pageVal = p.GetValueOrDefault();

            var movieList = await movieServices.GetMovies(paginationVal, pageVal - 1);
            var totalCount = await movieServices.GetMovieCount();

            var viewModel = new IndexViewModel() { Movies = movieList, Page = pageVal, TotalMovieCount = totalCount };

            return View(viewModel);
        }

        public async Task<IActionResult> Privacy()
        {
            //try
            //{   // Open the text file using a stream reader.
            //    using (StreamReader sr = new StreamReader("unew.item"))
            //    {
            //        // Read the stream to a string, and write the string to the console.
            //        while (!sr.EndOfStream)
            //        {

            //            string line = await sr.ReadLineAsync();
            //            var splitted = line.Split("|");

            //            Movie movie = new Movie()
            //            {
            //                Name = splitted[1],
            //                ReleaseDate = splitted[2],
            //                ImdbLink = splitted[4]
            //            };
            //            await movieServices.AddMovie(movie);
            //            Console.WriteLine(line);

            //        }
            //    }
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(e.Message);
            //}

            //Random random = new Random();
            //for (int i = 0; i < 943; i++)
            //{
            //    var username = "";
            //    var password = "";
            //    for (int j = 0; j < 6; j++)
            //    {
            //        username += (char)random.Next(67, 122);
            //        password += (char)random.Next(67, 122);
            //    }

            //    await this.userService.AddUser(new Models.User() { Username = username, Password = password });
            //}

            //var count = 0;
            //try
            //{   // Open the text file using a stream reader.
            //    using (StreamReader sr = new StreamReader("u.data"))
            //    {
            //        // Read the stream to a string, and write the string to the console.
            //        while (!sr.EndOfStream)
            //        {
            //            string line = await sr.ReadLineAsync();

            //            var splitted = line.Split("\t");
            //            var rates = new List<Rate>();

            //            Rate rate = new Rate()
            //            {
            //                UserId = int.Parse(splitted[0]),
            //                MovieId = int.Parse(splitted[1]),
            //                MovieRate = int.Parse(splitted[2])
            //            };

            //            movieContext.Rates.Add(rate);

            //            if (count % 50 == 0)
            //            {
            //                await movieContext.SaveChangesAsync();
            //                rates.Clear();
            //            }
            //            count++;
            //        }

            //    }

            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(e.Message);
            //}
            var user2 = await userService.GetUser(2);
            await ratingService.PredictMovieRate(user2,3);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
