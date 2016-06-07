using System.Windows.Forms;
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
            this.button1 = new System.Windows.Forms.Button();
            this.mainGrid = new System.Windows.Forms.DataGridView();
            this.welcomeTextBox = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.load_btn_Click);
            // 
            // mainGrid
            // 
            this.mainGrid.Location = new System.Drawing.Point(12, 32);
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.Size = new System.Drawing.Size(856, 456);
            this.mainGrid.TabIndex = 0;
            this.mainGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainGrid_CellContentClick);
            // 
            // welcomeTextBox
            // 
            this.welcomeTextBox.AutoSize = true;
            this.welcomeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.welcomeTextBox.Location = new System.Drawing.Point(241, 233);
            this.welcomeTextBox.Name = "welcomeTextBox";
            this.welcomeTextBox.Size = new System.Drawing.Size(416, 31);
            this.welcomeTextBox.TabIndex = 1;
            this.welcomeTextBox.Text = "Drag and Drop movie files/folders";
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(880, 500);
            this.Controls.Add(this.welcomeTextBox);
            this.Controls.Add(this.mainGrid);
            this.Controls.Add(this.button1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView mainGrid;
        private Label welcomeTextBox;
    }
}

