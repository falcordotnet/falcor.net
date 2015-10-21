namespace Falcor.Examples.Netflix.RecommendationService
{
    public class GenreResult : ServiceResult
    {
        private GenreResult(string error, string name) : base(error)
        {
            Name = name;
        }

        public string Name { get; }

        public static GenreResult SuccessResult(string genre) => new GenreResult(null, genre);
        public static GenreResult ErrorResult(string error) => new GenreResult(error, null);
    }
}