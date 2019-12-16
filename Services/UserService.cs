using Microsoft.EntityFrameworkCore;
using MovieRecommender.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRecommender.Services
{
    public class UserService : IUserService
    {

        private readonly MovieContext movieContext;

        public UserService(MovieContext movieContext)
        {
            this.movieContext = movieContext;
        }

        public async Task AddUser(User user)
        {
            await movieContext.Users.AddAsync(user);
            await movieContext.SaveChangesAsync();
        }

        public async Task<bool> CheckUser(string userName, string password)
        {
            var isUser = await movieContext.Users.FirstOrDefaultAsync(user => user.Username == userName && password == user.Password);

            if (isUser == null)
                return false;

            return true;
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await movieContext.Users.FirstOrDefaultAsync(us => us.Id == userId);

            if (user == null)
                return null;

            return user;
        }

        public async Task<User> GetUserByName(string username)
        {
            var user = await movieContext.Users
                            .Include(x => x.Rates)
                            .FirstOrDefaultAsync(user => user.Username == username);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers(int paginationSize, int page)
        {
            return await movieContext.Users.ToListAsync();
        }
    }
}
