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
using System.Drawing.Imaging;

namespace CheckMoviesOut
{
    public partial class MainWindow : Form
    {
        private MoviesController _controller;
        private int _rowctr;
        private List<Movie> _moviesCollection;
        ListView myListView;

        public MainWindow()
        {
            InitializeComponent();
            init_Table();
            _rowctr = 0;
            _controller = new MoviesController();
            _moviesCollection = new List<Movie>();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            AllowDrop = true;
            DragEnter += new DragEventHandler(MainWindow_DragEnter);
            DragDrop += new DragEventHandler(MainWindow_DragDrop);

            mainGrid.Visible = false;
            mainGrid.Anchor = AnchorStyles.Right;
            mainGrid.Dock = DockStyle.Fill;
            mainGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            mainGrid.AllowUserToDeleteRows = true;
            welcomeTextBox.Anchor = AnchorStyles.Left;
            welcomeTextBox.Anchor = AnchorStyles.Top;

            Anchor = AnchorStyles.Right;
            Anchor = AnchorStyles.Top;
            Dock = DockStyle.Fill;
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private async void MainWindow_DragDrop(object sender, DragEventArgs e)
        {        
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            welcomeTextBox.Visible = false;
            mainGrid.Visible = true;
            
            foreach (string file in files)
            {
                await generate_rows(file);
               
            }
            string v = "";
            foreach (var item in _moviesCollection)
            {

                v += item.FileName;

            }

        }

        public async Task generate_rows(string path)
        {

            List<string> directories = new List<string>();
            List<string> files = new List<string>();
            string[] allowedFormats = { "avi", "mp4", "mp3", "wmv", "m4v", "mpg", "mpeg", "flv", "rmvb", "mov", "mkv" };
            Movie movie = new Movie();

            string tit = "";
            string fullname;
            int l = path.Length;
            //if folder passed
            if (Directory.Exists(path))
            {

                directories = Directory.GetDirectories(path).ToList();
                files = Directory.GetFiles(path).ToList();
                foreach (var item in files)
                {
                    directories.Add(item);
                }

                foreach (var file in directories)
                {
                    fullname = file.Substring(l + 1);
                    if (fullname.Length > 3)
                    {                              
                        //string last4 = fullname.Substring(fullname.Length - 4).ToLower();
                        tit = _controller.filter_valid_title(fullname);
                        movie = await _controller.down_movie_desc(tit, fullname);
                        fillNewGridRow(movie);
                        _moviesCollection.Add(movie);
                        _rowctr++;
                    }
                }
            }
            else //a file
            {

                int ct = path.LastIndexOf('\\');
                fullname = path.Substring(ct + 1);
                tit = _controller.filter_valid_title(fullname);
                movie = await _controller.down_movie_desc(tit, fullname);
                fillNewGridRow(movie);
                _moviesCollection.Add(movie);
                _rowctr++;

            }
        }

        public void fillNewGridRow(Movie movie)
        {
            mainGrid.Rows.Add();

            mainGrid[0, _rowctr].Value = _rowctr;
            mainGrid[1, _rowctr].Value = movie?.Title;
            mainGrid[2, _rowctr].Value = movie?.Image;
            mainGrid[3, _rowctr].Value = movie?.Rating;
            mainGrid[4, _rowctr].Value = movie?.Votes;
            mainGrid[5, _rowctr].Value = movie?.Genre;
            mainGrid[6, _rowctr].Value = movie?.Director;
            mainGrid[7, _rowctr].Value = movie?.Plot;
            mainGrid[8, _rowctr].Value = movie?.Stars;
            mainGrid[9, _rowctr].Value = movie?.RealaseDate;
            mainGrid[10, _rowctr].Value = movie?.FileName;

            DataGridViewCell linkCell = new DataGridViewLinkCell();
            linkCell.Value = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movie.Title + "&s=all";
            mainGrid[11, _rowctr] = linkCell;

            mainGrid.Rows[_rowctr].Height = 100;
        }

        private void mainGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.RowIndex != -1)
            {
                string value = mainGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Process.Start(value);
            }
        }


        private async void loadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "C:\\";

            DialogResult result = fbd.ShowDialog();

            string sub = fbd.SelectedPath;

            if (sub != "C:\\")
            {
               // await _controller.generate_rows(sub);

            }
        }

        private void setCellProps(int col)
        {
            mainGrid.Columns[col].DefaultCellStyle.Font = new Font("Arial", 9F, GraphicsUnit.Pixel);
            mainGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            mainGrid.Columns[col].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void init_Table()
        {
            mainGrid.Columns.Add("id", "ID");
            mainGrid.Columns.Add("title", "TITLE");
            mainGrid.Columns.Add(new DataGridViewImageColumn { Name = "Image", Image = null, ImageLayout = DataGridViewImageCellLayout.Stretch });
            mainGrid.Columns.Add("rating", "RATING");
            mainGrid.Columns.Add("views", "VOTES");
            mainGrid.Columns.Add("genre", "GENRE");
            mainGrid.Columns.Add("director", "DIRECTOR");
            mainGrid.Columns.Add("plot", "PLOT");
            mainGrid.Columns.Add("actors", "STARS");
            mainGrid.Columns.Add("year", "REALASE DATE");
            mainGrid.Columns.Add("writer", "FILENAME");
            mainGrid.Columns.Add("titlex", "URL");

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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form();

            Label l = new Label();
            l.Text = "Hello world";

            f.Controls.Add(l);


            f.Show();
            
        }

        private void switchToViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainGrid.Visible = false;
            welcomeTextBox.Visible = false;


            myListView = new ListView();
            myListView.Dock = DockStyle.Fill;
            myListView.View = View.Tile;

            // Initialize the tile size.
            myListView.TileSize = new Size(300, 150);

            var imageList = new ImageList();
            foreach (var item in _moviesCollection)
            {


                // Initialize the item icons.
                //var myImageList = new ImageList();
                //using (Icon myIcon = new Icon("book.ico"))
                //{
                //    myImageList.Images.Add(myIcon);
                //}
                //myImageList.ImageSize = new Size(32, 32);
                //myListView.LargeImageList = myImageList;

                var movie = item;
                ListViewItem listViewItem;
                if (movie.Image != null)
                {
                    
                    imageList.Images.Add(movie.FileName, movie.Image);
                    imageList.ImageSize = new Size(60, 80);

                }

                string[] arr = new string[8];
                arr[0] = movie.Title;
                arr[1] = movie.Rating;
                arr[2] = movie.Plot;
                arr[3] = movie.RealaseDate;
                arr[4] = movie.Genre;
                arr[5] = movie.Stars;
                arr[6] = movie.Votes;
                arr[7] = movie.Url;
                

                ListViewItem itemek = new ListViewItem(arr, movie.FileName);
                
                //ListViewItem itemek = new ListViewItem(movie.Title);

                myListView.Items.Add(itemek);
                // Add column headers so the subitems will appear.
                myListView.Columns.AddRange(new ColumnHeader[]
                    { new ColumnHeader(), new ColumnHeader(), new ColumnHeader()});

                //// Create items and add them to myListView.
                //ListViewItem movieItem = new ListViewItem(new string[]
                //{
                // movie.Title,
                // movie.Rating,
                // movie.Votes});

                //myListView.Items.Add(movieItem);
               
            }
            myListView.LargeImageList = imageList;
            this.Controls.Add(myListView);
        }

        private void switchToGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myListView.Visible = false;

            mainGrid.Visible = true;
        }
    }
}
