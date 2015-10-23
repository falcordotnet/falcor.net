using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Falcor.Examples.Netflix.RatingService;
using Falcor.Examples.Netflix.RecommendationService;
using Falcor.Server;

namespace Falcor.Examples.Netflix
{
    public class NetflixRouter : FalcorRouter
    {
        public NetflixRouter(
            IRatingService ratingService,
            IRecommendationService recommendationService,
            int userId)
        {
            Get["titlesById[{ranges:titleIds}]['rating']"] = async parameters =>
            {
                List<int> titleIds = parameters.titleIds;
                var ratings = await ratingService.GetRatingsAsync(titleIds, userId);
                var results = new List<PathValue>();

                titleIds.ToList().ForEach(titleId =>
                {
                    var rating = ratings.SingleOrDefault(r => r.TitleId == titleId);

                    // Handle missing results
                    if (rating == null)
                    {
                        results.Add(Path("titlesById", titleId).Undefined());
                    }
                    // Handle errors
                    else if (rating.Error)
                    {
                        results.Add(Path("titlesById", rating.TitleId, "userRating").Error(rating.ErrorMessage));
                        results.Add(Path("titlesById", rating.TitleId, "rating").Error(rating.ErrorMessage));
                    }
                    else
                    {
                        results.Add(Path("titlesById", rating.TitleId, "userRating").Atom(rating.UserRating));
                        results.Add(Path("titlesById", rating.TitleId, "rating").Atom(rating.Rating));
                    }
                });

                return Complete(results);
            };

            Get["genrelist[{integers:indices}].name"] = async parameters =>
            {
                var genreResults = await recommendationService.GetGenreListAsync(userId);
                List<int> indices = parameters.indices;
                var results = indices.Select(index =>
                {
                    var genre = genreResults.ElementAtOrDefault(index);
                    return genre != null
                        ? Path("genrelist", index, "name").Atom(genre.Name, expires:-3000000)
                        : Path("genrelist", index).Undefined();
                });
                //return Complete(Path("genrelist", 0, "name").Error("test"));
                return Complete(results);
            };

            Get["genrelist.mylist"] = async _ =>
            {
                var genreResults = await recommendationService.GetGenreListAsync(userId);
                var myList = genreResults.SingleOrDefault(g => g.IsMyList);
                var index = genreResults.IndexOf(myList);
                return myList != null
                    ? Complete(Path("genrelist", "mylist").Ref("genrelist", index))
                    : Error("myList missing from genrelist");
            };


        }
    }
}
