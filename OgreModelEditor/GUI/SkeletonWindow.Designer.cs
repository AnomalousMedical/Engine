namespace OgreModelEditor
{
    partial class SkeletonWindow
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
            this.skeletonTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // skeletonTree
            // 
            this.skeletonTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skeletonTree.Location = new System.Drawing.Point(0, 0);
            this.skeletonTree.Name = "skeletonTree";
            this.skeletonTree.Size = new System.Drawing.Size(284, 264);
            this.skeletonTree.TabIndex = 0;
            // 
            // SkeletonWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.CloseButton = false;
            this.Controls.Add(this.skeletonTree);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "SkeletonWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.Text = "Skeleton";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView skeletonTree;
    }
}