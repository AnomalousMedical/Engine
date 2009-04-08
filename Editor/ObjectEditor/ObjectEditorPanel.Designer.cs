namespace Editor
{
    partial class ObjectEditorPanel
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
            this.treeTableSplit = new System.Windows.Forms.SplitContainer();
            this.editInterfaceView = new Editor.EditInterfaceView();
            this.propertiesTable = new Editor.PropertiesTable();
            this.treeTableSplit.Panel1.SuspendLayout();
            this.treeTableSplit.Panel2.SuspendLayout();
            this.treeTableSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeTableSplit
            // 
            this.treeTableSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTableSplit.Location = new System.Drawing.Point(0, 0);
            this.treeTableSplit.Name = "treeTableSplit";
            // 
            // treeTableSplit.Panel1
            // 
            this.treeTableSplit.Panel1.Controls.Add(this.editInterfaceView);
            // 
            // treeTableSplit.Panel2
            // 
            this.treeTableSplit.Panel2.Controls.Add(this.propertiesTable);
            this.treeTableSplit.Size = new System.Drawing.Size(783, 451);
            this.treeTableSplit.SplitterDistance = 194;
            this.treeTableSplit.TabIndex = 0;
            // 
            // editInterfaceView
            // 
            this.editInterfaceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editInterfaceView.Location = new System.Drawing.Point(0, 0);
            this.editInterfaceView.Name = "editInterfaceView";
            this.editInterfaceView.Size = new System.Drawing.Size(194, 451);
            this.editInterfaceView.TabIndex = 0;
            // 
            // propertiesTable
            // 
            this.propertiesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesTable.Location = new System.Drawing.Point(0, 0);
            this.propertiesTable.Name = "propertiesTable";
            this.propertiesTable.Size = new System.Drawing.Size(585, 451);
            this.propertiesTable.TabIndex = 0;
            // 
            // ObjectEditorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTableSplit);
            this.Name = "ObjectEditorPanel";
            this.Size = new System.Drawing.Size(783, 451);
            this.treeTableSplit.Panel1.ResumeLayout(false);
            this.treeTableSplit.Panel2.ResumeLayout(false);
            this.treeTableSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer treeTableSplit;
        private EditInterfaceView editInterfaceView;
        private PropertiesTable propertiesTable;
    }
}
