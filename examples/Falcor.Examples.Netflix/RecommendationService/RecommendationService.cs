using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Falcor.Examples.Netflix.RecommendationService
{
    public class RecommendationService : IRecommendationService
    {
        private static List<string> Genres { get; } = new List<string>
        {
            "Action",
            "Adventure",
            "Comedy",
            "Crime",
            "Fantasy",
            "Historical",
            "Historical fiction",
            "Horror/Thriller",
            "Mystery",
            "Paranoid",
            "Philosophical",
            "Political",
            "Romance",
            "Saga",
            "Satire",
            "Science fiction",
            "Thriller",
            "Animation"
        };

        public async Task<List<GenreResult>> GetGenreListAsync(int userId)
        {
            await Task.Delay(1);
            return Genres.Select(GenreResult.SuccessResult).ToList();
        }
    }
}