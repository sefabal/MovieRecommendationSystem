using System.Collections.Generic;

namespace MovieRecommender.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ReleaseDate { get; set; }

        public string ImdbLink { get; set; }

        public List<Rate> Rates { get; set; }
    }
}
