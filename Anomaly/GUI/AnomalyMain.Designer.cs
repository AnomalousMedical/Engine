namespace Anomaly
{
    partial class AnomalyMain
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
            this.objectViewSplit = new System.Windows.Forms.SplitContainer();
            this.objectViewSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectViewSplit
            // 
            this.objectViewSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectViewSplit.Location = new System.Drawing.Point(0, 0);
            this.objectViewSplit.Name = "objectViewSplit";
            this.objectViewSplit.Size = new System.Drawing.Size(752, 540);
            this.objectViewSplit.SplitterDistance = 250;
            this.objectViewSplit.TabIndex = 1;
            // 
            // AnomalyMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 540);
            this.Controls.Add(this.objectViewSplit);
            this.Name = "AnomalyMain";
            this.Text = "Anomaly";
            this.objectViewSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer objectViewSplit;

    }
}

