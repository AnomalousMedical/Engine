namespace Editor
{
    partial class SingleEnumEditor
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
            this.valueBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // valueBox
            // 
            this.valueBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueBox.FormattingEnabled = true;
            this.valueBox.Location = new System.Drawing.Point(0, 0);
            this.valueBox.Name = "valueBox";
            this.valueBox.Size = new System.Drawing.Size(150, 21);
            this.valueBox.TabIndex = 0;
            // 
            // SingleEnumEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueBox);
            this.Name = "SingleEnumEditor";
            this.Size = new System.Drawing.Size(150, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox valueBox;
    }
}
