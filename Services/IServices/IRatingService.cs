using MovieRecommender.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public interface IRatingService
    {
        Task AddRating(Rate rate);

        Task<List<Rate>> GetRates();

        Task AddBulkRating(IEnumerable<Rate> rates);

        Task<int> PredictMovieRate(User predictedUser, int movieIndex);
    }
}
