namespace Editor
{
    partial class DrawingWindowHost
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
            this.components = new System.ComponentModel.Container();
            this.tabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changeBackgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingWindow = new Editor.DrawingWindow();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.tabContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabContextMenu
            // 
            this.tabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBackgroundColorToolStripMenuItem});
            this.tabContextMenu.Name = "contextMenuStrip1";
            this.tabContextMenu.Size = new System.Drawing.Size(215, 48);
            // 
            // changeBackgroundColorToolStripMenuItem
            // 
            this.changeBackgroundColorToolStripMenuItem.Name = "changeBackgroundColorToolStripMenuItem";
            this.changeBackgroundColorToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.changeBackgroundColorToolStripMenuItem.Text = "Change Background Color";
            this.changeBackgroundColorToolStripMenuItem.Click += new System.EventHandler(this.changeBackgroundColorToolStripMenuItem_Click);
            // 
            // drawingWindow
            // 
            this.drawingWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingWindow.Location = new System.Drawing.Point(0, 0);
            this.drawingWindow.Name = "drawingWindow";
            this.drawingWindow.Size = new System.Drawing.Size(284, 264);
            this.drawingWindow.TabIndex = 0;
            // 
            // DrawingWindowHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.drawingWindow);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DrawingWindowHost";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.TabPageContextMenuStrip = this.tabContextMenu;
            this.Text = "SplitViewHost";
            this.tabContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DrawingWindow drawingWindow;
        private System.Windows.Forms.ContextMenuStrip tabContextMenu;
        private System.Windows.Forms.ToolStripMenuItem changeBackgroundColorToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}