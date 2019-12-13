using System.ComponentModel.DataAnnotations;

namespace MovieRecommender.Models
{
    public class Rate
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }

        public User User { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        [Range(0,5)]
        public int MovieRate { get; set; }
    }
}
