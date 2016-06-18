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
       
            movie = extract_cont(ret, fullname);

            return movie;

        }
    
        private Movie extract_cont(string json, string filename)
        {
            Movie m = new Movie();
            m.FileName = filename;

            JObject jObj = JObject.Parse(json);

            foreach (var item in jObj)
            {
                m.MovieTable.Add(item.Key, item.Value.ToString());
            }

            return m;
        }

    }
}

