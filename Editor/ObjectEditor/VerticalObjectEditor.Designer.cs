namespace Editor
{
    partial class VerticalObjectEditor
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
            this.propertiesTable = new Editor.PropertiesTable();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.editInterfaceView = new Editor.EditInterfaceView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertiesTable
            // 
            this.propertiesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesTable.Location = new System.Drawing.Point(0, 0);
            this.propertiesTable.Name = "propertiesTable";
            this.propertiesTable.Size = new System.Drawing.Size(230, 450);
            this.propertiesTable.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.editInterfaceView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertiesTable);
            this.splitContainer1.Size = new System.Drawing.Size(230, 641);
            this.splitContainer1.SplitterDistance = 187;
            this.splitContainer1.TabIndex = 1;
            // 
            // editInterfaceView
            // 
            this.editInterfaceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editInterfaceView.Location = new System.Drawing.Point(0, 0);
            this.editInterfaceView.Name = "editInterfaceView";
            this.editInterfaceView.Size = new System.Drawing.Size(230, 187);
            this.editInterfaceView.TabIndex = 0;
            // 
            // VerticalObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 641);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VerticalObjectEditor";
            this.Text = "VerticalObjectEditor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertiesTable propertiesTable;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private EditInterfaceView editInterfaceView;
    }
}