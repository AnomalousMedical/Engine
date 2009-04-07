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
            this.objectsTree = new System.Windows.Forms.TreeView();
            this.propertiesTable = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.editPropertyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.treeTableSplit.Panel1.SuspendLayout();
            this.treeTableSplit.Panel2.SuspendLayout();
            this.treeTableSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesTable)).BeginInit();
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
            this.treeTableSplit.Panel1.Controls.Add(this.objectsTree);
            // 
            // treeTableSplit.Panel2
            // 
            this.treeTableSplit.Panel2.Controls.Add(this.propertiesTable);
            this.treeTableSplit.Size = new System.Drawing.Size(783, 451);
            this.treeTableSplit.SplitterDistance = 194;
            this.treeTableSplit.TabIndex = 0;
            // 
            // objectsTree
            // 
            this.objectsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsTree.Location = new System.Drawing.Point(0, 0);
            this.objectsTree.Name = "objectsTree";
            this.objectsTree.Size = new System.Drawing.Size(194, 451);
            this.objectsTree.TabIndex = 0;
            // 
            // propertiesTable
            // 
            this.propertiesTable.AllowUserToAddRows = false;
            this.propertiesTable.AllowUserToDeleteRows = false;
            this.propertiesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propertiesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.valueColumn,
            this.editPropertyColumn});
            this.propertiesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesTable.Location = new System.Drawing.Point(0, 0);
            this.propertiesTable.Name = "propertiesTable";
            this.propertiesTable.Size = new System.Drawing.Size(585, 451);
            this.propertiesTable.TabIndex = 0;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // valueColumn
            // 
            this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valueColumn.HeaderText = "Value";
            this.valueColumn.Name = "valueColumn";
            // 
            // editPropertyColumn
            // 
            this.editPropertyColumn.HeaderText = "EditProperty";
            this.editPropertyColumn.Name = "editPropertyColumn";
            this.editPropertyColumn.Visible = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.propertiesTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer treeTableSplit;
        private System.Windows.Forms.TreeView objectsTree;
        private System.Windows.Forms.DataGridView propertiesTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn editPropertyColumn;
    }
}
