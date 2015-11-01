using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faker;

namespace Falcor.Examples.Netflix.RatingService
{
    public class FakeRatingService : IRatingService
    {
        public Task<List<RatingResult>> GetRatingsAsync(IEnumerable<int> titleIds, int userId)
        {
            return Task.FromResult(
                titleIds.Select(titleId =>
                    RatingResult.SuccessResult(titleId, RandomNumber.Next(0, 5), RandomNumber.Next(0, 5))).ToList());
        }
    }
}