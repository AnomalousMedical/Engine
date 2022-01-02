namespace ImageAtlasPacker
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAtlasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveIndexTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadIndexTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.imagePropertiesControl1 = new ImageAtlasPacker.ImagePropertiesControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picturePanel = new System.Windows.Forms.Panel();
            this.imageIndexControl1 = new ImageAtlasPacker.ImageIndexControl();
            this.openTemplateDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveTemplateDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.picturePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(939, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAtlasToolStripMenuItem,
            this.saveIndexTemplateToolStripMenuItem,
            this.loadIndexTemplateToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveAtlasToolStripMenuItem
            // 
            this.saveAtlasToolStripMenuItem.Name = "saveAtlasToolStripMenuItem";
            this.saveAtlasToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveAtlasToolStripMenuItem.Text = "Save Atlas";
            this.saveAtlasToolStripMenuItem.Click += new System.EventHandler(this.saveAtlasToolStripMenuItem_Click);
            // 
            // saveIndexTemplateToolStripMenuItem
            // 
            this.saveIndexTemplateToolStripMenuItem.Name = "saveIndexTemplateToolStripMenuItem";
            this.saveIndexTemplateToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveIndexTemplateToolStripMenuItem.Text = "Save Index Template";
            this.saveIndexTemplateToolStripMenuItem.Click += new System.EventHandler(this.saveIndexTemplateToolStripMenuItem_Click);
            // 
            // loadIndexTemplateToolStripMenuItem
            // 
            this.loadIndexTemplateToolStripMenuItem.Name = "loadIndexTemplateToolStripMenuItem";
            this.loadIndexTemplateToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.loadIndexTemplateToolStripMenuItem.Text = "Load Index Template";
            this.loadIndexTemplateToolStripMenuItem.Click += new System.EventHandler(this.loadIndexTemplateToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "JPEG(*.jpg)|*.jpg;|PNG(*.png)|*.png;|TIFF(*.tiff)|*.tiff;|BMP(*.bmp)|*.bmp;";
            this.saveImageDialog.FilterIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imageIndexControl1);
            this.splitContainer1.Size = new System.Drawing.Size(939, 565);
            this.splitContainer1.SplitterDistance = 677;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.imagePropertiesControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.picturePanel);
            this.splitContainer2.Size = new System.Drawing.Size(677, 565);
            this.splitContainer2.SplitterDistance = 257;
            this.splitContainer2.TabIndex = 1;
            // 
            // imagePropertiesControl1
            // 
            this.imagePropertiesControl1.AllowDrop = true;
            this.imagePropertiesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePropertiesControl1.Location = new System.Drawing.Point(0, 0);
            this.imagePropertiesControl1.Name = "imagePropertiesControl1";
            this.imagePropertiesControl1.PictureBox = this.pictureBox1;
            this.imagePropertiesControl1.Size = new System.Drawing.Size(257, 565);
            this.imagePropertiesControl1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // picturePanel
            // 
            this.picturePanel.AutoScroll = true;
            this.picturePanel.Controls.Add(this.pictureBox1);
            this.picturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picturePanel.Location = new System.Drawing.Point(0, 0);
            this.picturePanel.Name = "picturePanel";
            this.picturePanel.Size = new System.Drawing.Size(416, 565);
            this.picturePanel.TabIndex = 0;
            // 
            // imageIndexControl1
            // 
            this.imageIndexControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageIndexControl1.FooterText = "";
            this.imageIndexControl1.HeaderText = "";
            this.imageIndexControl1.IndexText = "";
            this.imageIndexControl1.Location = new System.Drawing.Point(0, 0);
            this.imageIndexControl1.Name = "imageIndexControl1";
            this.imageIndexControl1.Size = new System.Drawing.Size(258, 565);
            this.imageIndexControl1.TabIndex = 0;
            // 
            // openTemplateDialog
            // 
            this.openTemplateDialog.Filter = "Text Files(*.txt)|*.txt;";
            // 
            // saveTemplateDialog
            // 
            this.saveTemplateDialog.Filter = "Text Files(*.txt)|*.txt;";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 589);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Image Atlas Packer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.picturePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAtlasToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ImagePropertiesControl imagePropertiesControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel picturePanel;
        private ImageIndexControl imageIndexControl1;
        private System.Windows.Forms.ToolStripMenuItem saveIndexTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadIndexTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openTemplateDialog;
        private System.Windows.Forms.SaveFileDialog saveTemplateDialog;
    }
}

