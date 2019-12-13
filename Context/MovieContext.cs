using Microsoft.EntityFrameworkCore;
using MovieRecommender.Models;

namespace MovieRecommender
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }
    }
}
