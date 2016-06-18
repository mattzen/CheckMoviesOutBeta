using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckMoviesOut
{
    public class Movie
    {
        public Dictionary<string, string> MovieTable;

        public Movie()
        {
            MovieTable = new Dictionary<string, string>();
        }

        public class DictionaryKeys
        {
            public const string TitleKey = "Title";
            public const string RatingKey = "imdbRating";
            public const string VotesKey = "imdbVotes";
            public const string GenreKey = "Genre";
            public const string DirectorKey = "Director";
            public const string PlotKey = "Plot";
            public const string ActorsKey = "Actors";
            public const string ReleaseDateKey = "Year";
            public const string WriterKey = "Writer";
            public const string ImageKey = "Poster";
        }

        public int Id { get; set; }
        public string Title { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.TitleKey, string.Empty); } set { } }
        public Image Image { get { return getImage(Infra.GetValueOrDefault(MovieTable, DictionaryKeys.ImageKey, string.Empty), Title); } set { } }
        public string ImageUrl { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.ImageKey, string.Empty); } set { } }
        public string Rating { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.RatingKey, string.Empty); } set { } }
        public string Votes { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.VotesKey, string.Empty); } set { } }
        public string Genre { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.GenreKey, string.Empty); } set { } }
        public string Director { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.DirectorKey, string.Empty); } set { } }
        public string Writer { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.WriterKey, string.Empty); } set { } }
        public string Plot { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.PlotKey, string.Empty); } set { } }
        public string Stars { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.ActorsKey, string.Empty); } set { } }
        public string RealaseDate { get { return Infra.GetValueOrDefault(MovieTable, DictionaryKeys.ReleaseDateKey, string.Empty); } set { } }
        public string FileName { get; set; }
        public string Url { get; set; }

        public Image getImage(string url, string tit)
        {
            if (string.IsNullOrEmpty(url) || url == "N/A") return null;

            Image img;
            string dir = Directory.GetCurrentDirectory();
            string imgs = dir + "/imgs/";

            if (!File.Exists(imgs))
            {
                Directory.CreateDirectory(imgs);
            }

            string curFile = imgs + tit.Replace(":","").Replace("*","") + ".jpg";

            if (!File.Exists(curFile))
            {

                WebRequest requestPic = WebRequest.Create(url);
                WebResponse responsePic = requestPic.GetResponse();
                Image webImage = Image.FromStream(responsePic.GetResponseStream()); // Error
                webImage.Save(curFile);
                img = webImage;

            }
            else
            {
                Image webImage = Image.FromFile(curFile);
                img  = webImage;
            }

            return img;
        }

    }
}
