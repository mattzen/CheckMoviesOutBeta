﻿using System;
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
        private List<Tuple<string,string>> _movieTitlesYears;
        private List<Tuple<string, string>> _unknownMovieTitlesYears;

        private List<Movie> _moviesCollection;
        ListView myListView;
        private List<Movie> _unknownMovies;
        List<string> _files;
        private string currentPathLocation = string.Empty;
        List<Movie> movieCollection;

        List<Movie> foundMovies;

        //MAIN ENTRY POINT
        private async void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                welcomeTextBox.Visible = false;
                mainGrid.Visible = true;

                foreach (string file in files)
                {
                    await generate_rows(file);

                }
            }
            catch (Exception er)
            {

            }

        }



        public MainWindow()
        {
            InitializeComponent();
            init_Table();
            _rowctr = 0;
            _controller = new MoviesController();
            _moviesCollection = new List<Movie>();
            _unknownMovies = new List<Movie>();
            _movieTitlesYears = new List<Tuple<string, string>>();
            movieCollection = new List<Movie>();

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


      

        public void traverse(string path)
        {
            currentPathLocation += path+"/";
            if (Directory.Exists(path))
            {
                var directories = Directory.GetDirectories(path).ToList();
                var files = Directory.GetFiles(path).ToList();
                _files.AddRange(files);

                foreach (var item in directories)
                {
                    traverse(item);
                }
            }
            else
            {
                _files.Add(path);                
            }
        }

        public async Task generate_rows(string path)
        {
            _files = new List<string>();
            traverse(path);
            var files = _files;
            movieCollection = _controller.LoadJson();
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                if (_controller.isInCollection(fileName))
                {
                    var movie1 = movieCollection.FirstOrDefault(x => x.FileName == fileName);

                    fillNewGridRow(movie1);
                    _moviesCollection.Add(movie1);
                    _rowctr++;
                    continue;
                }

                var movieItem = _controller.GetTitleAndYear(fileName);

                if (string.IsNullOrEmpty(movieItem?.Item1) || _movieTitlesYears.Contains(movieItem))
                {
                    continue;
                }

                _movieTitlesYears.Add(movieItem);
                var loc = path + "\\" + fileName;
                var movie = await _controller.GetMovie(movieItem.Item1, fileName, movieItem.Item2, loc);
                movie.Image = await _controller.GetImage(movie.ImageUrl, movie.Title);
                fillNewGridRow(movie);
                _moviesCollection.Add(movie);
                _rowctr++;
            }
        }


        //Main Grid

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
            linkCell.Value = movie.Url;
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


        //List View
        private void MyListView_ItemActivate(object sender, EventArgs e)
        {
            Form f = new Form();
            f.BackColor = Color.White;
            f.FormClosing += F_FormClosing;

            PictureBox px = new PictureBox();

            Label Title = new Label();
            Label Rating = new Label();
            Label Director = new Label();
            Label Plot = new Label();
            Label RealaseDate = new Label();
            Label Genre = new Label();
            Label Stars = new Label();
            LinkLabel Url = new LinkLabel();

            Label openFile = new Label();

            LinkLabel Location = new LinkLabel();

            f.Size = new Size(810, 460);
            ListViewItem item = ((ListView)sender).SelectedItems[0];
            Image img = item.ImageList.Images[item.ImageKey];


            Title.Text = item.SubItems[0].Text.ToString();
            Rating.Text = item.SubItems[1].Text.ToString() + " (" + item.SubItems[6].Text.ToString() + " votes)";
            Plot.Text = item.SubItems[2].Text.ToString();
            RealaseDate.Text = item.SubItems[3].Text.ToString();
            Genre.Text = item.SubItems[4].Text.ToString();
            Stars.Text = item.SubItems[5].Text.ToString();
            Director.Text = item.SubItems[7].Text.ToString();
            Url.Text = item.SubItems[8].Text.ToString();
            openFile.Text = "play movie";

            Url.Click += Url_Click;
            openFile.Click += (senderr, ee) => OpenFileClick(senderr, ee, item.SubItems[10].Text.ToString());


    

            Title.AutoSize = true;
            Title.Font = new Font("arial", 16);
            Title.Location = new Point(260, 10);

            RealaseDate.AutoSize = true;
            RealaseDate.Location = new Point(260, 40);

            Genre.AutoSize = true;
            Genre.Location = new Point(260, 60);

            Director.AutoSize = true;
            Director.Location = new Point(260, 80);


            Rating.AutoSize = true;
            Rating.Location = new Point(260, 100);


            Stars.Location = new Point(260, 120);
            Stars.Width = 600;



            Plot.Font = new Font("arial", 16);
            Plot.Width = 550;
            Plot.Height = 150;
            Plot.Location = new Point(260, 150);

  


            Url.Location = new Point(260, 390);
            Url.Width = 550;

            openFile.Font = new Font("arial", 40);
            openFile.Width = 550;
            openFile.Height = 150;
            openFile.Location = new Point(260, 300);



            px.Location = new Point(10, 10);
            px.SetBounds(10, 10, 250, 400);

            //img.Size = new Size(200, 500);
            px.SizeMode = PictureBoxSizeMode.StretchImage;
            px.Image = img;

            f.Controls.Add(px);

            f.Controls.Add(Title);
            f.Controls.Add(Rating);
            f.Controls.Add(Plot);
            f.Controls.Add(RealaseDate);
            f.Controls.Add(Genre);
            f.Controls.Add(Stars);
            f.Controls.Add(Director);
            f.Controls.Add(Url);
            f.Controls.Add(openFile);

            f.Show();
            f.PerformLayout();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenFileClick(object sender, EventArgs e, string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch
            {

            }
        }

        private void Url_Click(object sender, EventArgs e)
        {
            var a = (LinkLabel)sender;
            Process.Start(a.Text);
        }

        private void tILESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainGrid.Visible = false;
            welcomeTextBox.Visible = false;


            myListView = new ListView();
            myListView.Dock = DockStyle.Fill;
            myListView.View = View.Tile;
            myListView.ItemActivate += MyListView_ItemActivate;

            // Initialize the tile size.
            myListView.TileSize = new Size(400, 300);
            myListView.ShowItemToolTips = true;
            var imageList = new ImageList();

            myListView.Columns.AddRange(new ColumnHeader[]
                 { new ColumnHeader(), new ColumnHeader(), new ColumnHeader(), new ColumnHeader(), new ColumnHeader(), new ColumnHeader(), new ColumnHeader(), new ColumnHeader()});


            foreach (var item in _moviesCollection.OrderByDescending(x => x.Rating))
            {

                var movie = item;
                ListViewItem listViewItem;
                if (movie.Image != null)
                {

                    imageList.Images.Add(movie.FileName, movie.Image);
                    imageList.ImageSize = new Size(150, 250);
                    string[] arr = new string[11];
                    arr[0] = movie.Title;
                    arr[1] = movie.Rating;
                    arr[2] = movie.Plot;
                    arr[3] = movie.RealaseDate;
                    arr[4] = movie.Genre;
                    arr[5] = movie.Stars;
                    arr[6] = movie.Votes;
                    arr[7] = movie.Director;
                    arr[8] = movie.Url;
                    arr[9] = movie.Writer;
                    arr[10] = movie.Location;
                    ListViewItem itemek = new ListViewItem(arr, movie.FileName);
                    myListView.Items.Add(itemek);
                }
                else
                {

                }



            }
            myListView.LargeImageList = imageList;
            this.Controls.Add(myListView);
        }





        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void F_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }



        private void switchToGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myListView.Visible = false;

            mainGrid.Visible = true;
        }

     

        private void gRIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myListView==null || myListView.Visible == false) return;
            myListView.Visible = false;

            mainGrid.Visible = true;
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void loadLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            welcomeTextBox.Visible = false;
            mainGrid.Visible = true;

            movieCollection = _controller.LoadJson();

            if (movieCollection == null) return;

            foreach (var movie in movieCollection)
            {
                fillNewGridRow(movie);
                _moviesCollection.Add(movie);
                _rowctr++;
            }

        }


        public void YearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void AlphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void RatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imageList = new ImageList();
            myListView.Items.Clear();
            var coll = foundMovies;
            foreach (var item in coll.OrderByDescending(x=>x.Rating))
            {
                var movie = item;
                if (movie.Image != null)
                {
                    imageList.Images.Add(movie.FileName, movie.Image);
                    imageList.ImageSize = new Size(150, 250);
                    string[] arr = new string[10];
                    arr[0] = movie.Title;
                    arr[1] = movie.Rating;
                    arr[2] = movie.Plot;
                    arr[3] = movie.RealaseDate;
                    arr[4] = movie.Genre;
                    arr[5] = movie.Stars;
                    arr[6] = movie.Votes;
                    arr[7] = movie.Director;
                    arr[8] = movie.Url;
                    arr[9] = movie.Writer;
                    ListViewItem itemek = new ListViewItem(arr, movie.FileName);
                    myListView.Items.Add(itemek);
                }
                else if (item.MovieTable.Count > 9)
                {
                    string[] arr = new string[10];
                    arr[0] = movie.Title;
                    arr[1] = movie.Rating;
                    arr[2] = movie.Plot;
                    arr[3] = movie.RealaseDate;
                    arr[4] = movie.Genre;
                    arr[5] = movie.Stars;
                    arr[6] = movie.Votes;
                    arr[7] = movie.Director;
                    arr[8] = movie.Url;
                    arr[9] = movie.Writer;
                    ListViewItem itemek = new ListViewItem(arr, movie.FileName);
                    myListView.Items.Add(itemek);

                }
            }
            myListView.LargeImageList = imageList;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {

            if (mainGrid.Visible != true)
            {
                var val = searchBox.Text.ToUpper();
                var imageList = new ImageList();
                myListView.Items.Clear();
                foundMovies = new List<Movie>();
                foreach (var movie in _moviesCollection)
                {
                    if (movie.Title.ToUpper().Contains(val) ||
                        movie.Genre.ToUpper().Contains(val) ||
                        movie.Plot.ToUpper().Contains(val) ||
                        movie.Stars.ToUpper().Contains(val) ||
                        movie.Rating.Contains(val) ||
                        movie.RealaseDate.Contains(val))
                    {
                        foundMovies.Add(movie);
                    }
                }
                var coll = foundMovies;
                foreach (var item in coll)
                {
                    var movie = item;
                    ListViewItem listViewItem;
                    if (movie.Image != null)
                    {

                        imageList.Images.Add(movie.FileName, movie.Image);
                        imageList.ImageSize = new Size(150, 250);
                        string[] arr = new string[10];
                        arr[0] = movie.Title;
                        arr[1] = movie.Rating;
                        arr[2] = movie.Plot;
                        arr[3] = movie.RealaseDate;
                        arr[4] = movie.Genre;
                        arr[5] = movie.Stars;
                        arr[6] = movie.Votes;
                        arr[7] = movie.Director;
                        arr[8] = movie.Url;
                        arr[9] = movie.Writer;
                        ListViewItem itemek = new ListViewItem(arr, movie.FileName);
                        myListView.Items.Add(itemek);
                    }
                    else if (item.MovieTable.Count > 9)
                    {
                        string[] arr = new string[10];
                        arr[0] = movie.Title;
                        arr[1] = movie.Rating;
                        arr[2] = movie.Plot;
                        arr[3] = movie.RealaseDate;
                        arr[4] = movie.Genre;
                        arr[5] = movie.Stars;
                        arr[6] = movie.Votes;
                        arr[7] = movie.Director;
                        arr[8] = movie.Url;
                        arr[9] = movie.Writer;
                        ListViewItem itemek = new ListViewItem(arr, movie.FileName);
                        myListView.Items.Add(itemek);

                    }


                }
                myListView.LargeImageList = imageList;
            }
        }



    }
}
