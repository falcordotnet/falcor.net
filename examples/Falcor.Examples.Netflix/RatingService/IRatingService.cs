using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Examples.Netflix.RatingService
{
    public interface IRatingService
    {
        Task<List<RatingResult>> GetRatingsAsync(IEnumerable<int> titleIds, int userId);
    }
}