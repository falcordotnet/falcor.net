namespace Falcor.Examples.Netflix.RecommendationService
{
    public class GenreResult : ServiceResult
    {
        private GenreResult(string error, string name, bool isMyList) : base(error)
        {
            Name = name;
            IsMyList = isMyList;
        }

        public string Name { get; }
        public bool IsMyList { get; }
        public static GenreResult SuccessResult(string genre) => new GenreResult(null, genre, false);
        public static GenreResult SuccessResult(string genre, bool isMyList) => new GenreResult(null, genre, isMyList);
        public static GenreResult ErrorResult(string error) => new GenreResult(error, null, false);
    }
}