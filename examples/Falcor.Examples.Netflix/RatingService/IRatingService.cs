using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Examples.Netflix.RatingService
{
    public interface IRatingService
    {
        Task<List<RatingResult<int>>> GetRatingsAsync(IEnumerable<int> titleIds, int userId);
		Task<List<RatingResult<Guid>>> GetRatingsAsync(IEnumerable<Guid> titleIds, int userId);
	}
}