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
 
        private MoviesController controller;

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


            controller = new MoviesController(this.mainGrid);

               
           
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private async void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
           
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var type = e.Data.GetType();
            

            foreach (string file in files)
            {
                await controller.generate_rows(file);
               
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

            //string 

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "C:\\";

            DialogResult result = fbd.ShowDialog();

            string sub = fbd.SelectedPath;


            if (sub != "C:\\")
            {
                await controller.generate_rows(sub);

            }
        }

       
    

    }
}
