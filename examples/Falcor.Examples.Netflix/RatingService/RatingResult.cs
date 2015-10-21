namespace Falcor.Examples.Netflix.RatingService
{
    public class RatingResult : ServiceResult
    {
        private RatingResult(int titleId, int userRating, int rating) : this(titleId, null)
        {
            UserRating = userRating;
            Rating = rating;
        }

        private RatingResult(int titleId, string error) : base(error)
        {
            TitleId = titleId;
        }

        public int TitleId { get; }
        public int UserRating { get; }
        public int Rating { get; }

        public static RatingResult SuccessResult(int titleId, int userRating, int rating) => new RatingResult(titleId, userRating, rating);
        public static RatingResult ErrorResult(int titleId, string error) => new RatingResult(titleId, error);
    }
}