using MovieRecommender.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public interface IUserService
    {
        Task AddUser(User user);

        Task<IEnumerable<User>> GetUsers(int paginationSize, int page);

        Task<User> GetUser(int userId);

        Task<bool> CheckUser(string userName, string password);
    }
}
