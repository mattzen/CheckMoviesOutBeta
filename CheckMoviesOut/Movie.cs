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

        public class DicrtionaryKeys
        {
            public const string TitleName = "Title";
            public const string RatingName = "imdbRating";
            public const string VotesName = "imdbVotes";
            public const string GenreName = "Genre";
            public const string DirectorName = "Director";
            public const string PlotName = "Plot";
            public const string ActorsName = "Actors";
            public const string YearName = "Year";
            public const string WriterName = "Writer";
            public const string PosterName = "Poster";
        }

        public int Id { get; set; }
        public string Title { get { return MovieDict[DicrtionaryKeys.TitleName]; } set { } }
        public Image Image { get; set; }
        public string Rating { get { return MovieDict[DicrtionaryKeys.RatingName]; } set { } }
        public string Votes { get { return MovieDict[DicrtionaryKeys.VotesName]; } set { } }
        public string Genre { get { return MovieDict[DicrtionaryKeys.GenreName]; } set { } }
        public string Director { get { return MovieDict[DicrtionaryKeys.DirectorName]; } set { } }
        public string Writer { get { return MovieDict[DicrtionaryKeys.WriterName]; } set { } }
        public string Plot { get { return MovieDict[DicrtionaryKeys.PlotName]; } set { } }
        public string Stars { get { return MovieDict[DicrtionaryKeys.ActorsName]; } set { } }
        public string RealaseDate { get { return MovieDict[DicrtionaryKeys.YearName]; } set { } }
        public string FileName { get; set; }
        public string Url { get; set; }


        public Dictionary<string, string> MovieDict = new Dictionary<string, string>();


    }
}
