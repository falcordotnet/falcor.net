using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Falcor.Examples.Netflix.RatingService
{
    public class FakeRatingService : IRatingService
    {
        public Task<List<RatingResult>> GetRatingsAsync(IEnumerable<int> titleIds, int userId)
        {
            return Task.FromResult(
                titleIds.Select(titleId =>
                    RatingResult.SuccessResult(titleId, Faker.RandomNumber.Next(0, 5), Faker.RandomNumber.Next(0, 5))).ToList());
        }
    }
}