using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMediaLite.Data;


using MyMediaLite.DataType;
using MyMediaLite.RatingPrediction;
namespace Model
{
    class Engine
    {
        public static List<Movie> GetRecommendations(List<Movie> all, Dictionary<int, int> newRanks, List<Tuple<int, int, float>> oldRanks)
        {
            int newUserId = 0;
            Ratings ratings = new Ratings();

            foreach (var r in oldRanks)
            {
                ratings.Add(r.Item1, r.Item2, r.Item3);
                if (r.Item1 > newUserId) newUserId = r.Item1;
            }

            newUserId = newUserId + 1;

            foreach (var k in newRanks)
            {
                ratings.Add(newUserId, k.Key, (float)k.Value);
            }

            var engine = new BiPolarSlopeOne();
            engine.Ratings = ratings;

            engine.Train();

            return all.Select(m => { m.Rank = engine.CanPredict(newUserId, m.Id) ? engine.Predict(newUserId, m.Id) : -1; return m; }).ToList();
        }
    }
}
