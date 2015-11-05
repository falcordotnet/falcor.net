using System;

namespace Falcor.Examples.Netflix.RatingService
{
    public class RatingResult : ServiceResult
    {
        private RatingResult(Guid titleId, int userRating, int rating) : this(titleId, null)
        {
            UserRating = userRating;
            Rating = rating;
        }

        private RatingResult(Guid titleId, string error) : base(error)
        {
            TitleId = titleId;
        }

        public Guid TitleId { get; }
        public int UserRating { get; }
        public int Rating { get; }

        public static RatingResult SuccessResult(Guid titleId, int userRating, int rating)
            => new RatingResult(titleId, userRating, rating);

        public static RatingResult ErrorResult(Guid titleId, string error) => new RatingResult(titleId, error);
    }
}