using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Examples.Netflix.RecommendationService
{
    public interface IRecommendationService
    {
        Task<List<GenreResult>> GetGenreListAsync(int userId);
    }
}