using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faker;

namespace Falcor.Examples.Netflix.RatingService
{
    public class FakeRatingService : IRatingService
    {
        public Task<List<RatingResult<int>>> GetRatingsAsync(IEnumerable<int> titleIds, int userId)
        {
            return Task.FromResult(
                titleIds.Select(titleId =>
                    RatingResult<int>.SuccessResult(titleId, RandomNumber.Next(0, 5), RandomNumber.Next(0, 5))).ToList());
        }

		public Task<List<RatingResult<Guid>>> GetRatingsAsync(IEnumerable<Guid> titleIds, int userId) {
			return Task.FromResult(
				titleIds.Select(titleId =>
					RatingResult<Guid>.SuccessResult(titleId, RandomNumber.Next(0, 5), RandomNumber.Next(0, 5))).ToList());
		}
	}
}