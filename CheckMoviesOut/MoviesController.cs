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

namespace CheckMoviesOut
{
    public class MoviesController
    {

        public MoviesController()
        {

        }

        public string filter_valid_title(string title)
        {

            string tit = title;

            Regex expression2 = new Regex(@"[A-Z]*[a-z]*");
            var results2 = expression2.Matches(tit.ToString());

            string build = "";

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

                        build += match2.ToString() + "";

                        if (ctr % 2 == 0)
                        {
                            build += "%20";
                        }
                    }
                }
                ctr++;
            }

            return build;

        }

        public async Task<Movie> down_movie_desc(string title, string fullname)
        {
            string req = "http://www.omdbapi.com/?t=" + title;

            string ret = "";

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

                }
                throw ex;
            }

            Movie movie = new Movie();
       
            movie = extract_cont( ret, fullname);

            return movie;

        }

        private Movie extract_cont(string json, string tit)
        {
            Movie movie = new Movie();
            movie.FileName = tit;

            List<string> keywords = new List<string>() { 
                "Title",
                "imdbRating",
                "imdbVotes",
                "Genre",
                "Director",
                "Plot",
                "Actors",
                "Year",
                "Writer",
                "Poster" };

            string value = "";
            foreach (var keyword in keywords)
            {

                string exp = keyword;
                Regex expression2 = new Regex(@exp + ".*");
                int offset = exp.Length + 3;

                tit = tit.Replace("%20", "");


                var results = expression2.Matches(json.ToString());
                
                foreach (Match singleMatch in results)
                {

                    string match = singleMatch.ToString();
                   

                    for (int i = 0; i < match.Length; i++)
                    {

                        if (match[offset + i] == '"')
                        {
                            break;
                        }
                        value += match[offset + i];
                    }
                }

                    if (keyword == "Title") movie.Title = value;
                    else if (keyword == "imdbRating") movie.Rating = value;
                    else if (keyword == "imdbVotes") movie.Votes = value;
                    else if (keyword == "Genre") movie.Genre = value;
                    else if (keyword == "Director") movie.Director = value;
                    else if (keyword == "Plot") movie.Plot = value;
                    else if (keyword == "Actors") movie.Stars = value;
                    else if (keyword == "Year") movie.RealaseDate = value;
                    else if (keyword == "Writer") movie.Writer = value;
                    else if (keyword == "Poster" && value != "N/A" && value != "")
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string imgs = dir + "/imgs/";

                        if (!File.Exists(imgs))
                        {
                            Directory.CreateDirectory(imgs);
                        }

                        string curFile = imgs + tit + ".jpg";

                        if (!File.Exists(curFile))
                        {

                            WebRequest requestPic = WebRequest.Create(value);
                            WebResponse responsePic = requestPic.GetResponse();
                            Image webImage = Image.FromStream(responsePic.GetResponseStream()); // Error
                            webImage.Save(curFile);
                            movie.Image = webImage;

                        }
                        else
                        {
                            Image webImage = Image.FromFile(curFile);
                            movie.Image = webImage;
                        }

                    }
                    else
                    {

                        string link = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + value + "&s=all";
                        movie.Url = link;
                    }

                value = "";
                  

                }

                return movie;

            }
        }

    }

