using MovieRecommender.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public interface IMovieServices
    {
        Task<bool> AddMovie(Movie movie);

        Task<IEnumerable<Movie>> GetMovies(int paginationSize,int page);

        Task<int> GetMovieCount();
    }
}
