
namespace testiä
{
    partial class imageMTDT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(imageMTDT));
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DirectoryBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showpng = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ChosenFolder = new System.Windows.Forms.TextBox();
            this.FileBox = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CurrentFile = new System.Windows.Forms.TextBox();
            this.currentimage = new System.Windows.Forms.NumericUpDown();
            this.TagSearch = new System.Windows.Forms.TextBox();
            this.refreshbtn = new System.Windows.Forms.Button();
            this.imageAmount2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentimage)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox
            // 
            this.PictureBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DirectoryBtn,
            this.toolStripSeparator1,
            this.showpng});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // DirectoryBtn
            // 
            this.DirectoryBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.DirectoryBtn, "DirectoryBtn");
            this.DirectoryBtn.Name = "DirectoryBtn";
            this.DirectoryBtn.Click += new System.EventHandler(this.ToolStripButton1_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // showpng
            // 
            this.showpng.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.showpng, "showpng");
            this.showpng.Name = "showpng";
            this.showpng.Click += new System.EventHandler(this.showpng_Click);
            // 
            // ChosenFolder
            // 
            this.ChosenFolder.AllowDrop = true;
            resources.ApplyResources(this.ChosenFolder, "ChosenFolder");
            this.ChosenFolder.Name = "ChosenFolder";
            this.ChosenFolder.ReadOnly = true;
            // 
            // FileBox
            // 
            this.FileBox.AllowDrop = true;
            this.FileBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.FileBox.HideSelection = false;
            resources.ApplyResources(this.FileBox, "FileBox");
            this.FileBox.Name = "FileBox";
            this.FileBox.UseCompatibleStateImageBehavior = false;
            this.FileBox.View = System.Windows.Forms.View.Details;
            this.FileBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FileBox_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // CurrentFile
            // 
            resources.ApplyResources(this.CurrentFile, "CurrentFile");
            this.CurrentFile.Name = "CurrentFile";
            this.CurrentFile.ReadOnly = true;
            // 
            // currentimage
            // 
            this.currentimage.BackColor = System.Drawing.SystemColors.Control;
            this.currentimage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.currentimage, "currentimage");
            this.currentimage.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.currentimage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.currentimage.Name = "currentimage";
            this.currentimage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.currentimage_KeyPress);
            // 
            // TagSearch
            // 
            resources.ApplyResources(this.TagSearch, "TagSearch");
            this.TagSearch.Name = "TagSearch";
            this.TagSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TagSearch_MouseClick);
            this.TagSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TagSearch_KeyPress);
            // 
            // refreshbtn
            // 
            resources.ApplyResources(this.refreshbtn, "refreshbtn");
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.UseVisualStyleBackColor = true;
            this.refreshbtn.Click += new System.EventHandler(this.refreshbtn_Click);
            // 
            // imageAmount2
            // 
            resources.ApplyResources(this.imageAmount2, "imageAmount2");
            this.imageAmount2.Name = "imageAmount2";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // imageMTDT
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.imageAmount2);
            this.Controls.Add(this.refreshbtn);
            this.Controls.Add(this.TagSearch);
            this.Controls.Add(this.currentimage);
            this.Controls.Add(this.CurrentFile);
            this.Controls.Add(this.FileBox);
            this.Controls.Add(this.ChosenFolder);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.PictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "imageMTDT";
            this.Load += new System.EventHandler(this.imageMTDT_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.start_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.start_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentimage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton DirectoryBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox ChosenFolder;
        private System.Windows.Forms.ListView FileBox;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox CurrentFile;
        private System.Windows.Forms.NumericUpDown currentimage;
        private System.Windows.Forms.TextBox TagSearch;
        private System.Windows.Forms.Button refreshbtn;
        private System.Windows.Forms.ToolStripButton showpng;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label imageAmount2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

