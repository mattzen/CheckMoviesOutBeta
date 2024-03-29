﻿using System.Windows.Forms;
namespace CheckMoviesOut
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainGrid = new System.Windows.Forms.DataGridView();
            this.welcomeTextBox = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gRIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tILESToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchButton = new System.Windows.Forms.Button();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.ratingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGrid
            // 
            this.mainGrid.BackgroundColor = System.Drawing.Color.White;
            this.mainGrid.Location = new System.Drawing.Point(15, 30);
            this.mainGrid.MinimumSize = new System.Drawing.Size(600, 400);
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.Size = new System.Drawing.Size(856, 417);
            this.mainGrid.TabIndex = 0;
            this.mainGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainGrid_CellContentClick);
            // 
            // welcomeTextBox
            // 
            this.welcomeTextBox.AutoSize = true;
            this.welcomeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.welcomeTextBox.Location = new System.Drawing.Point(202, 222);
            this.welcomeTextBox.Name = "welcomeTextBox";
            this.welcomeTextBox.Size = new System.Drawing.Size(416, 31);
            this.welcomeTextBox.TabIndex = 1;
            this.welcomeTextBox.Text = "Drag and Drop movie files/folders";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gRIDToolStripMenuItem,
            this.tILESToolStripMenuItem,
            this.loadLibraryToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gRIDToolStripMenuItem
            // 
            this.gRIDToolStripMenuItem.Name = "gRIDToolStripMenuItem";
            this.gRIDToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.gRIDToolStripMenuItem.Text = "GRID";
            this.gRIDToolStripMenuItem.Click += new System.EventHandler(this.gRIDToolStripMenuItem_Click);
            // 
            // tILESToolStripMenuItem
            // 
            this.tILESToolStripMenuItem.Name = "tILESToolStripMenuItem";
            this.tILESToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.tILESToolStripMenuItem.Text = "TILES";
            this.tILESToolStripMenuItem.Click += new System.EventHandler(this.tILESToolStripMenuItem_Click);
            // 
            // loadLibraryToolStripMenuItem
            // 
            this.loadLibraryToolStripMenuItem.Name = "loadLibraryToolStripMenuItem";
            this.loadLibraryToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.loadLibraryToolStripMenuItem.Text = "Load Library";
            this.loadLibraryToolStripMenuItem.Click += new System.EventHandler(this.loadLibraryToolStripMenuItem_Click);
            // 
            // searchButton
            // 
            this.searchButton.ForeColor = System.Drawing.Color.Black;
            this.searchButton.Location = new System.Drawing.Point(572, 2);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(116, 21);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ratingToolStripMenuItem,
            this.yearToolStripMenuItem,
            this.alphToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(62, 20);
            this.toolStripMenuItem1.Text = "Sorty By";
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(397, 3);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(169, 20);
            this.searchBox.TabIndex = 3;

            // ratingToolStripMenuItem
            // 
            this.ratingToolStripMenuItem.Name = "ratingToolStripMenuItem";
            this.ratingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ratingToolStripMenuItem.Text = "Rating";
            this.ratingToolStripMenuItem.Click += RatingToolStripMenuItem_Click;
            // 
            // yearToolStripMenuItem
            // 
            this.yearToolStripMenuItem.Name = "yearToolStripMenuItem";
            this.yearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.yearToolStripMenuItem.Text = "Year";
            this.yearToolStripMenuItem.Click += YearToolStripMenuItem_Click;
            // 
            // alphToolStripMenuItem
            // 
            this.alphToolStripMenuItem.Name = "alphToolStripMenuItem";
            this.alphToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.alphToolStripMenuItem.Text = "Alph";
            this.alphToolStripMenuItem.Click += AlphToolStripMenuItem_Click;
            // 

            // MainWindow
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(883, 478);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.welcomeTextBox);
            this.Controls.Add(this.mainGrid);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataGridView mainGrid;
        private Label welcomeTextBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem gRIDToolStripMenuItem;
        private ToolStripMenuItem tILESToolStripMenuItem;
        private ToolStripMenuItem loadLibraryToolStripMenuItem;
        private Button searchButton;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem ratingToolStripMenuItem;
        private ToolStripMenuItem yearToolStripMenuItem;
        private ToolStripMenuItem alphToolStripMenuItem;
        private TextBox searchBox;
    }
}

