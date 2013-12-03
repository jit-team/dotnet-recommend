using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    class MovieLoader
    {
        public static List<Model.Movie> Read(string filename)
        {
            List<Model.Movie> result = new List<Model.Movie>();

            System.IO.StreamReader myFile = new System.IO.StreamReader(filename);
            while (myFile.EndOfStream == false)
            {
                string line = myFile.ReadLine();
                var m = line.Split('|');
                result.Add(new Model.Movie() { Id = int.Parse(m[0]), Title = m[1], Link = m[4] });
            }


            return result;
        }

        public static List<Tuple<int, int, float>> ReadPredictions(string filename)
        {
            List<Tuple<int, int, float>> result = new List<Tuple<int, int, float>>();
            System.IO.StreamReader myFile = new System.IO.StreamReader(filename);
            while (myFile.EndOfStream == false)
            {
                string line = myFile.ReadLine();
                var m = line.Split('\t');
                result.Add(Tuple.Create(int.Parse(m[0]), int.Parse(m[1]), float.Parse(m[2])));
            }
            return result;
        }
    }
}
