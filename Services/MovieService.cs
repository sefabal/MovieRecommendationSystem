using KNNCalculation;
using Microsoft.EntityFrameworkCore;
using MovieRecommender.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public class MovieService : IMovieServices
    {
        private readonly MovieContext movieContext;

        private readonly CalculateKNN knnCalculator;

        public MovieService(MovieContext context, CalculateKNN knnCalculator)
        {
            this.movieContext = context;
            this.knnCalculator = knnCalculator;
        }

        public async Task<bool> AddMovie(Movie movie)
        {
            await this.movieContext.AddAsync(movie);
            await this.movieContext.SaveChangesAsync();

            return true;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await movieContext.Movies.FirstOrDefaultAsync(mov => mov.Id == id);
        }

        public async Task<int> GetMovieCount()
        {
            return await movieContext.Movies.CountAsync();
        }

        public async Task<IEnumerable<Movie>> GetMovies(int paginationSize, int page)
        {
            var movieList = await movieContext.Movies.Skip(page * paginationSize).Take(paginationSize).ToListAsync();

            return movieList;
        }
    }
}
