using System.Collections.Generic;

namespace MovieRecommender.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<Rate> Rates { get; set; }
    }
}
