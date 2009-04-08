namespace Editor
{
    partial class EditInterfaceView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectsTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // objectsTree
            // 
            this.objectsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsTree.Location = new System.Drawing.Point(0, 0);
            this.objectsTree.Name = "objectsTree";
            this.objectsTree.Size = new System.Drawing.Size(213, 429);
            this.objectsTree.TabIndex = 1;
            // 
            // EditInterfaceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectsTree);
            this.Name = "EditInterfaceView";
            this.Size = new System.Drawing.Size(213, 429);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView objectsTree;
    }
}
