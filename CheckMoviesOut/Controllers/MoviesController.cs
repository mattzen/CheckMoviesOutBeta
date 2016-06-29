using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CheckMoviesOut
{
    public class MoviesController
    {

        List<Movie> movieCollection;


        public MoviesController()
        {

        }

     
        public Tuple<string, string> GetTitleAndYear(string filename)
        {
            string title = filename;
            string[] allowedFormats = { "avi", "mp4", "mp3", "wmv", "m4v", "mpg", "mpeg", "flv", "rmvb", "mov", "mkv" };

            if (title.Length > 3)
            {
                if(title[title.Length-4] == '.')
                {
                    if (!allowedFormats.Contains(title.Substring(title.Length - 3,3).ToLower())
                        || title.Substring(0,title.Length-4).ToLower() == "sample"
                        || title.Substring(0, title.Length - 4).ToLower() == "etrg")
                        return null;
                }
            }
            else
            {
                return null;
            }

            Regex reg = new Regex(@"[a-z]*");
            var result = reg.Matches(title.ToLower().ToString());

            string Title = "";

            int ctr = 1;
            foreach (Match match in result)
            {

                if (ctr <= 10)
                {
                    if (match.ToString().ToLower() != "dvdrip"
                        && match.ToString().ToLower() != "bdrip"
                        && match.ToString().ToLower() != "webrip"
                        && match.ToString().ToLower() != "hdrip"
                        && match.ToString().ToLower() != "extended"
                        && match.ToString().ToLower() != "limited"
                        && match.ToString().ToLower() != "xvid"
                        && match.ToString().ToLower() != "dvdrip"
                        && match.ToString().ToLower() != "hdrip"
                        && match.ToString().ToLower() != "xvi"
                        && match.ToString().ToLower() != "brrip"
                        && match.ToString().ToLower() != "sdtv"
                        && match.ToString().ToLower() != "dvdscr"
                        && match.ToString().ToLower() != "hc"
                        && match.ToString().ToLower() != "hdts"
                         && match.ToString().ToLower() != "hdtv"
                        )
                    {

                        Title += match.ToString() + "";

                        if (ctr % 2 == 0)
                        {
                            Title += " ";
                        }
                    }
                }
                ctr++;
            }

            Title = Title.Replace("  ", " ");

            var Year = GetMovieYear(filename);

            return new Tuple<string, string>(Title, Year);
        }

        private string GetMovieYear(string filename)
        {
            Regex expression2 = new Regex(@"\d{4}");
            var results2 = expression2.Matches(filename.ToLower().ToString());

            if (results2.Count == 0) return string.Empty;

            string build = "";

            int ctr = 1;
            foreach (Match match2 in results2)
            {
                build = match2.Value.ToString();
            }

            

            var year = int.Parse(build);

            if (year > 1950 && year < 2020)
            { }
            else { build = string.Empty; }

            return build;
        }

        public async Task<Movie> GetMovie(string title, string filename, string year)
        {          

            string req = "http://www.omdbapi.com/?t=" + title;

            if (!string.IsNullOrEmpty(year)) req += "&y=" + year;


            string ret = "";

            Movie movie = new Movie();

            var request = await WebRequest.Create(req).GetResponseAsync();
            try
            {
                using (Stream responseStream = request.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    ret = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    movie.Plot = errorText;
                }
                throw ex;
            }

            
       
            movie = ExtractJson(ret, filename, title,year);

            return movie;

        }
        public Image GetImageLocal(string url, string tit)
        {
            if (string.IsNullOrEmpty(url) || url == "N/A") return null;

            Image img;
            string dir = Directory.GetCurrentDirectory();
            string imgs = dir + "/imgs/";

            if (!File.Exists(imgs))
            {
                Directory.CreateDirectory(imgs);
            }

            string curFile = imgs + tit.Replace(":", "").Replace("*", "") + ".jpg";

            {
                Image webImage = Image.FromFile(curFile);
                img = webImage;
            }

            return img;
        }

        public async Task<Image> GetImage(string url, string tit)
        {
            if (string.IsNullOrEmpty(url) || url == "N/A") return null;

            Image img;
            string dir = Directory.GetCurrentDirectory();
            string imgs = dir + "/imgs/";

            if (!File.Exists(imgs))
            {
                Directory.CreateDirectory(imgs);
            }

            string curFile = imgs + tit.Replace(":", "").Replace("*", "") + ".jpg";

            if (!File.Exists(curFile))
            {

                WebRequest requestPic = WebRequest.Create(url);
                //WebResponse responsePic = requestPic.GetResponse();
                //Image webImage = Image.FromStream(responsePic.GetResponseStream()); // Error

                var response = await requestPic.GetResponseAsync();
                Image webImage = Image.FromStream(response.GetResponseStream());

                webImage.Save(curFile);
                img = webImage;

            }
            else
            {
                Image webImage = Image.FromFile(curFile);
                img = webImage;
            }

            return img;
        }

        public Movie ExtractJson(string json, string filename, string title, string year)
        {
            Movie movie = new Movie();
            movie.FileName = filename;

            JObject jObj = JObject.Parse(json);


            foreach (var item in jObj)
            {
                movie.MovieTable.Add(item.Key, item.Value.ToString());
            }

            movie.Url = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movie.Title + "&s=all&y="+year;

            if (string.IsNullOrEmpty(movie.Title))
            {
                movie.MovieTable["Title"] = title;
                movie.MovieTable["Year"] = year;
                movie.Url = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + title + "&s=all&y=" + year;
            }
            else
            {
                if (!isInCollection(filename))
                    SaveJson(json, filename);
            }
            return movie;
        }


        public List<Movie> LoadJson()
        {
            movieCollection = new List<Movie>();

            using (StreamReader file = File.OpenText(@"c:\\Users\\Matt\\Desktop\\hello.json"))
            {
                string fil = file.ReadToEnd();

                var split = fil?.Split('\n');

                if (split == null || split?[0] == string.Empty) return null;

                var newsplit = split.Take(split.Length - 1).ToList();

                foreach (var itemek in newsplit)
                {

                    Movie m = new Movie();
                    JObject o2 = (JObject)JToken.Parse(itemek);
                    foreach (var item in o2)
                    {
                        m.MovieTable.Add(item.Key, item.Value.ToString());
                    }
                    m.Image = GetImageLocal(m.ImageUrl, m.Title);
                    movieCollection.Add(m);
                }
            }
            return movieCollection;

        }

        public bool isInCollection(string filename)
        {
            return movieCollection.Exists(x => x.FileName == filename);
        }

        public void SaveJson(string json, string filename)
        {
                JObject jObj = JObject.Parse(json);
                jObj.Add("filename", filename);

                using (StreamWriter file = File.AppendText(@"c:\\Users\\Matt\\Desktop\\hello.json"))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jObj.WriteTo(writer);
                    file.Write('\n');
                }

        }

    }
}

