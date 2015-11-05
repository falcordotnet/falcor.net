using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Examples.Netflix.RatingService
{
    public interface IRatingService
    {
        Task<List<RatingResult>> GetRatingsAsync(IEnumerable<Guid> titleIds, int userId);
    }
}