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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fourWaySplit1 = new Anomaly.GUI.View.FourWaySplit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fourWaySplit1);
            this.splitContainer1.Size = new System.Drawing.Size(752, 540);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 1;
            // 
            // fourWaySplit1
            // 
            this.fourWaySplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fourWaySplit1.Location = new System.Drawing.Point(0, 0);
            this.fourWaySplit1.Name = "fourWaySplit1";
            this.fourWaySplit1.Size = new System.Drawing.Size(498, 540);
            this.fourWaySplit1.TabIndex = 0;
            // 
            // AnomalyMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 540);
            this.Controls.Add(this.splitContainer1);
            this.Name = "AnomalyMain";
            this.Text = "Anomaly";
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Anomaly.GUI.View.FourWaySplit fourWaySplit1;

    }
}

