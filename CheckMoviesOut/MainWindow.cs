using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CheckMoviesOut
{
    public partial class MainWindow : Form
    {
        int rowctr = 0;
        int lastrowctr =0;
        public MainWindow()
        {
            InitializeComponent();
            init_Table();

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //test branch
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MainWindow_DragEnter);
            this.DragDrop += new DragEventHandler(MainWindow_DragDrop);
            mainGrid.CellContentClick += new DataGridViewCellEventHandler(mainGrid_CellContentClick);
           
        }

        public void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        public async void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
           
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var type = e.Data.GetType();
           

            foreach (string file in files) await generate_tables(file);

        }

        public void setCellProps(int col)
        {

            mainGrid.Columns[col].DefaultCellStyle.Font = new Font("Arial", 9F, GraphicsUnit.Pixel);
            mainGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            mainGrid.Columns[col].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        }

        public void init_Table( )
        {

            mainGrid.Columns.Add("id", "id");
            mainGrid.Columns.Add("title", "title");

            mainGrid.Columns.Add(new DataGridViewImageColumn { Name = "Image", Image = null, ImageLayout= DataGridViewImageCellLayout.Stretch });
            
            mainGrid.Columns.Add("rating", "rating");
            mainGrid.Columns.Add("views", "views");
            mainGrid.Columns.Add("genre", "genre");
            mainGrid.Columns.Add("director", "director");
            mainGrid.Columns.Add("plot", "plot");
            mainGrid.Columns.Add("acotors", "actors");
            mainGrid.Columns.Add("year", "year");
            mainGrid.Columns.Add("writer", "writer");
            mainGrid.Columns.Add("titlex", "titlex");

            mainGrid.Columns[0].Width = 20;
            mainGrid.Columns[1].Width = 80;
            mainGrid.Columns[2].Width = 80;

            mainGrid.Columns[3].Width = 50;
            mainGrid.Columns[4].Width = 50;
            mainGrid.Columns[5].Width = 30;
            mainGrid.Columns[8].Width = 60;
            mainGrid.Columns[9].Width = 40;
            mainGrid.Columns[11].Width = 90;

            
           
            for (int i = 0; i < 12; i++)
            {
                setCellProps(i);
            }
            

         


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

        private async Task generate_tables(string sub)
        {

            lastrowctr = rowctr;
     
            List<string> directories = new List<string>();
            List<string> files = new List<string>();
            string[] allowedFormats = { "avi", "mp4", "mp3", "wmv", "m4v", "mpg", "mpeg", "flv", "rmvb", "mov", "mkv" };
            string tit = "";
            string fullname;
            int l = sub.Length;
            //if folder passed
            if (Directory.Exists(sub))
            {

                directories = Directory.GetDirectories(sub).ToList();
                files = Directory.GetFiles(sub).ToList();
                foreach (var item in files)
                {
                    directories.Add(item);
                }

            

                foreach (var file in directories)
                {

 
                        fullname = file.Substring(l + 1);
                        string last4 = fullname.Substring(fullname.Length - 4).ToLower();
                    
                           



                            tit = filter_valid_title(fullname);

                            await down_movie_desc(tit, fullname, rowctr);

                            rowctr++;

}
            }
            else //a file
            {

                        int ct = sub.LastIndexOf('\\');

                        fullname = sub.Substring(ct + 1);
               
                        tit = filter_valid_title(fullname);

                        await down_movie_desc(tit, fullname, rowctr);

                        rowctr++;
                    
            }
           

   
   
        }

        public async Task<string> down_movie_desc(string title, string fullname, int rowctr)
        {
            string req = "http://www.omdbapi.com/?t=" + title;

            string ret = "";

            var request = await WebRequest.Create(req).GetResponseAsync();
            try
            {
      

                using (Stream responseStream = request.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    ret =  reader.ReadToEnd();
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
                throw;
            }

            mainGrid.Rows.Add();


            extract_cont("Title", ret, 1, rowctr, fullname);
            extract_cont("imdbRating", ret, 3, rowctr, fullname);
            extract_cont("imdbVotes", ret, 4, rowctr, fullname);
            extract_cont("Genre", ret, 5, rowctr, fullname);
            extract_cont("Director", ret, 6, rowctr, fullname);
            extract_cont("Plot", ret, 7, rowctr, fullname);
            extract_cont("Actors", ret, 8, rowctr, fullname);
            extract_cont("Year", ret, 9, rowctr, fullname);
            extract_cont("Poster", ret, 10, rowctr, fullname);
            extract_cont("Writer", ret, 10, rowctr, fullname);



            mainGrid.Rows[rowctr].Height = 100;

            DataGridViewCell linkCell = new DataGridViewLinkCell();
            linkCell.Value = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + title + "&s=all";
            mainGrid[11, rowctr] = linkCell;

            mainGrid[10, rowctr].Value = fullname;
            mainGrid[0, rowctr].Value = rowctr;



            return ret;

        }

        void mainGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.RowIndex != -1)
            {
                
                string value = mainGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Process.Start(value);
            }
        }

        private void load_btn_Click(object sender, EventArgs e)
        {

            //string 
        
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "C:\\";

            DialogResult result = fbd.ShowDialog();

            string sub = fbd.SelectedPath;


            if (sub != "C:\\")
            {
                generate_tables(sub);

            }
        }

        public int check_resp(string s)
        {


                string exp = "Response";
                Regex expression2 = new Regex(@exp+".*");
                int offset = exp.Length + 3;
                

                var results2 = expression2.Matches(s.ToString());
                string build = "";
                foreach (Match match2 in results2)
                {

                    string match = match2.ToString();

                   
                    for (int i = 0; i < match.Length; i++)
                    {



                        if (match[offset + i] == '"')
                        {
                            break;
                        }
                        build += match[offset + i];
                    }
                }

            if(build == "False")
                return 0;
            else
                return 1;



        }
       
        public void extract_cont(string e, string s, int col, int row, string tit)
        {

                string exp = e;
                Regex expression2 = new Regex(@exp+".*");
                int offset = exp.Length + 3;

                tit = tit.Replace("%20", "");


                var results2 = expression2.Matches(s.ToString());

                foreach (Match match2 in results2)
                {

                    string match = match2.ToString();             
                
                    string build = "";

                    for (int i = 0; i < match.Length; i++)
                    {

                        

                        if (match[offset + i] == '"')
                        {
                            break;
                        }
                        build += match[offset + i];
                    }


                    if (e == "Poster" && build != "N/A")
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

                            WebRequest requestPic = WebRequest.Create(build);
                            WebResponse responsePic = requestPic.GetResponse();
                            Image webImage = Image.FromStream(responsePic.GetResponseStream()); // Error
                            webImage.Save(curFile);
                            mainGrid["Image", row].Value = webImage;
                            mainGrid[col, row].Value = webImage;
                        }
                        else
                        {
                            Image webImage = Image.FromFile(curFile);
                            mainGrid["Image", row].Value = webImage;
                            mainGrid[col, row].Value = webImage;

                        }

                    }
                    else
                    {

                        if (col == 11)
                        {

                            mainGrid[col, row].Value = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + build + "&s=all";
                        }
                        else
                            mainGrid[col, row].Value = build;

                        

                    }
                }

       


        }

    }
}
