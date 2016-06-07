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
        private MoviesController _controller;

        public MainWindow()
        {
            InitializeComponent();
            init_Table();

            _controller = new MoviesController(mainGrid);
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
                await _controller.generate_rows(file);
               
            }
        }

        private void setCellProps(int col)
        {
            mainGrid.Columns[col].DefaultCellStyle.Font = new Font("Arial", 9F, GraphicsUnit.Pixel);
            mainGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            mainGrid.Columns[col].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void init_Table( )
        {
            mainGrid.Columns.Add("id", "ID");
            mainGrid.Columns.Add("title", "TITLE");
            mainGrid.Columns.Add(new DataGridViewImageColumn { Name = "Image", Image = null, ImageLayout= DataGridViewImageCellLayout.Stretch });           
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

        private void mainGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.RowIndex != -1)
            {
                string value = mainGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Process.Start(value);
            }
        }

        private async void load_btn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "C:\\";

            DialogResult result = fbd.ShowDialog();

            string sub = fbd.SelectedPath;

            if (sub != "C:\\")
            {
                await _controller.generate_rows(sub);

            }
        }
    }
}
