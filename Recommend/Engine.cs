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

            // this makes us sure that the new user has a unique id (bigger than all other)
            newUserId = newUserId + 1;

            foreach (var k in newRanks)
            {
                ratings.Add(newUserId, k.Key, (float)k.Value);
            }

            var engine = new BiPolarSlopeOne();

            // different algorithm:
            // var engine = new UserItemBaseline();

            engine.Ratings = ratings;

            engine.Train(); // warning: this could take some time!

            return all.Select(m => 
                                { 
                                    m.Rank = engine.Predict(newUserId, m.Id); // do the prediction!
                                    return m; 
                                }).ToList();
        }
    }
}
