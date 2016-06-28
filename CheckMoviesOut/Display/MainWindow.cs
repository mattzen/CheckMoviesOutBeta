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
        private List<Movie> _unknownMovies;
        List<string> _files;

        public MainWindow()
        {
            InitializeComponent();
            init_Table();
            _rowctr = 0;
            _controller = new MoviesController();
            _moviesCollection = new List<Movie>();
            _unknownMovies = new List<Movie>();


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

        public void traverse(string path)
        {
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

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var movieItem = _controller.GetTitleAndYear(fileName);
                if (string.IsNullOrEmpty(movieItem?.Item1)) continue;
                var movie = await _controller.GetMovie(movieItem.Item1, fileName, movieItem.Item2);
                movie.Image = await _controller.GetImage(movie.ImageUrl, movie.Title);
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




        private void switchToViewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MyListView_ItemActivate(object sender, EventArgs e)
        {
            Form f = new Form();

            f.FormClosing += F_FormClosing;

            PictureBox px = new PictureBox();

            Label Title = new Label();
            Label Rating = new Label();
            Label Director = new Label();
            Label Plot = new Label();
            Label RealaseDate = new Label();
            Label Genre = new Label();
            Label Stars = new Label();
            Label Votes = new Label();
            Label Url = new Label();

            f.Size = new Size(800, 500);
            ListViewItem item = ((ListView)sender).SelectedItems[0];    
            Image img = item.ImageList.Images[item.ImageKey];


            Title.Text = item.SubItems[0].Text.ToString();
            Rating.Text = item.SubItems[1].Text.ToString();
            Plot.Text = item.SubItems[2].Text.ToString();
            RealaseDate.Text = item.SubItems[3].Text.ToString();
           
            Genre.Text = item.SubItems[4].Text.ToString();
            Stars.Text = item.SubItems[5].Text.ToString();
            Votes.Text = item.SubItems[6].Text.ToString();
            Director.Text = item.SubItems[7].Text.ToString();


            Title.AutoSize = true;
            Title.Location = new Point(200, 100);

            Rating.AutoSize = true;
            Rating.Location = new Point(200, 120);
          
            Stars.Location = new Point(200, 140);
            Stars.Width = 600;
            Director.Location = new Point(200, 160);
            Director.Width = 600;

            Plot.Font = new Font("arial", 16);
            Plot.Width = 600;
            Plot.Height = 200;
            Plot.Location = new Point(200, 180);

            Genre.AutoSize = true;
            Genre.Location = new Point(200, 380);

            RealaseDate.AutoSize = true;
            RealaseDate.Location = new Point(200, 400);

            Votes.AutoSize = true;
            Votes.Location = new Point(200, 420);

            px.Location = new Point(0, 0);
            px.SetBounds(0, 0, 200, 400);

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

            //f.Controls.Add(Votes);
            //f.Controls.Add(Url);

            f.Show();
            f.PerformLayout();
        }

        private void F_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void switchToGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myListView.Visible = false;

            mainGrid.Visible = true;
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
                else
                {

                }



            }
            myListView.LargeImageList = imageList;
            this.Controls.Add(myListView);
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
    }
}
