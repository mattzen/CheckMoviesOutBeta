using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMoviesOut
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Image Image { get; set; }
        public string Rating { get; set; }
        public string Votes { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Plot { get; set; }
        public string Stars { get; set; }
        public string RealaseDate { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }

    }
}
