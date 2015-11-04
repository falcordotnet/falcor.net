using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Examples.Netflix.RatingService;
using Falcor.Examples.Netflix.RecommendationService;
using Falcor.Server;

namespace Falcor.Examples.Netflix
{
    public class NetflixFalcorRouter : FalcorRouter
    {
        public NetflixFalcorRouter(IRatingService ratingService, IRecommendationService recommendationService,
            int userId)
        {
			Get["titlesById[{ranges:titleIds}]['rating', 'userRating']"] = async parameters => 
			{
				List<int> titleIds = parameters.titleIds;
				var ratings = await ratingService.GetRatingsAsync(titleIds, userId);
				var results = titleIds.Select(titleId => 
				{
					var rating = ratings.SingleOrDefault(r => r.TitleId == titleId);
					var path = Path("titlesById", titleId);
					if (rating == null) return path.Keys("userRating", "rating").Undefined();
					if (rating.Error) return path.Keys("userRating", "rating").Error(rating.ErrorMessage);
					return path
						.Key("userRating").Atom(rating.UserRating, TimeSpan.FromSeconds(-3))
						.Key("rating").Atom(rating.Rating);
				});

				return Complete(results);
			};

			Get["titlesById[{keys:ids}][{keys:props}]"] = async parameters => {
				KeySet idsSet = parameters.ids;
				KeySet propsSet = parameters.props;

				var ids = idsSet.Select(x => Guid.Parse(x.ToString()));
				var ratings = await ratingService.GetRatingsAsync(ids, userId);

				var results = ids.Select(titleId => {
					var rating = ratings.SingleOrDefault(r => r.TitleId == titleId);
					var path = Path("titlesById", titleId.ToString());
					if (rating == null) return path.Keys("userRating", "rating").Undefined();
					if (rating.Error) return path.Keys("userRating", "rating").Error(rating.ErrorMessage);
					return path
						.Key("userRating").Atom(rating.UserRating, TimeSpan.FromSeconds(-3))
						.Key("rating").Atom(rating.Rating);
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
						? Path("genrelist", index, "name").Atom(genre.Name)
						: Path("genrelist", index).Undefined();
				});
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