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
     
        public Tuple<string, string> GetTitleAndYear(string filename)
        {

            string tit = filename;

            string[] allowedFormats = { "avi", "mp4", "mp3", "wmv", "m4v", "mpg", "mpeg", "flv", "rmvb", "mov", "mkv" };
            if (tit.Length > 3)
            {
                if(tit[tit.Length-4] == '.')
                {
                    if (!allowedFormats.Contains(tit.Substring(tit.Length - 3,3).ToLower()) || tit.Substring(0,tit.Length-4).ToLower() == "sample") return null;
                }
            }
            else
            {
                return null;
            }


            Regex expression2 = new Regex(@"[a-z]*");
            var results2 = expression2.Matches(tit.ToLower().ToString());

            string Title = "";

            int ctr = 1;
            foreach (Match match2 in results2)
            {

                if (ctr <= 10)
                {
                    if (match2.ToString().ToLower() != "dvdrip"
                        && match2.ToString().ToLower() != "bdrip"
                        && match2.ToString().ToLower() != "webrip"
                        && match2.ToString().ToLower() != "hdrip"
                        && match2.ToString().ToLower() != "extended"
                        && match2.ToString().ToLower() != "limited"
                        && match2.ToString().ToLower() != "xvid"
                        && match2.ToString().ToLower() != "dvdrip"
                        && match2.ToString().ToLower() != "hdrip"
                        && match2.ToString().ToLower() != "xvi"
                        && match2.ToString().ToLower() != "brrip"
                        && match2.ToString().ToLower() != "sdtv"
                        && match2.ToString().ToLower() != "dvdscr"
                        && match2.ToString().ToLower() != "hc"
                        && match2.ToString().ToLower() != "hdts"
                         && match2.ToString().ToLower() != "hdtv"
                        )
                    {

                        Title += match2.ToString() + "";

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


            //
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

        private Movie ExtractJson(string json, string filename, string title, string year)
        {
            Movie m = new Movie();
            m.FileName = filename;

            JObject jObj = JObject.Parse(json);

            foreach (var item in jObj)
            {
                m.MovieTable.Add(item.Key, item.Value.ToString());
            }

            m.Url = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + m.Title + "&s=all&y="+year;

            if (string.IsNullOrEmpty(m.Title))
            {
                m.MovieTable["Title"] = title;
                m.MovieTable["Year"] = year;
                m.Url = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + title + "&s=all&y="+year;
            }


            return m;
        }

    }
}

