using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRecommender.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }

        public int TotalMovieCount { get; set; }

        public int Page { get; set; }
    }
}
