namespace Falcor.Examples.Netflix.RatingService
{
    public class RatingResult<TId> : ServiceResult
    {
        private RatingResult(TId titleId, int userRating, int rating) : this(titleId, null)
        {
            UserRating = userRating;
            Rating = rating;
        }

        private RatingResult(TId titleId, string error) : base(error)
        {
            TitleId = titleId;
        }

        public TId TitleId { get; }
        public int UserRating { get; }
        public int Rating { get; }

        public static RatingResult<TId> SuccessResult(TId titleId, int userRating, int rating)
            => new RatingResult<TId>(titleId, userRating, rating);

        public static RatingResult<TId> ErrorResult(TId titleId, string error) => new RatingResult<TId>(titleId, error);
    }
}