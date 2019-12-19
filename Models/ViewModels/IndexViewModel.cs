using System.Collections.Generic;

namespace MovieRecommender.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }

        public int TotalMovieCount { get; set; }

        public int Page { get; set; }

        public bool IsLoggedIn { get; set; }

        public Rate rate { get; set; }
    }
}
