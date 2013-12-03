using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }

        public float Rank { get; set; }

        public override string ToString()
        {
            return Title.ToString();
        }
    }
}
