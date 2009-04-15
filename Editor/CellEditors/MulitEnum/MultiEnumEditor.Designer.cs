namespace Editor
{
    partial class MultiEnumEditor
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
            this.multiEnumListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // multiEnumListBox
            // 
            this.multiEnumListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiEnumListBox.FormattingEnabled = true;
            this.multiEnumListBox.Location = new System.Drawing.Point(0, 0);
            this.multiEnumListBox.Name = "multiEnumListBox";
            this.multiEnumListBox.Size = new System.Drawing.Size(136, 19);
            this.multiEnumListBox.TabIndex = 0;
            // 
            // MultiEnumEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.multiEnumListBox);
            this.Name = "MultiEnumEditor";
            this.Size = new System.Drawing.Size(136, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox multiEnumListBox;
    }
}
