namespace Anomaly
{
    partial class SimObjectPanel
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
            this.editInterfaceView = new Editor.EditInterfaceView();
            this.SuspendLayout();
            // 
            // editInterfaceView
            // 
            this.editInterfaceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editInterfaceView.Location = new System.Drawing.Point(0, 0);
            this.editInterfaceView.Name = "editInterfaceView";
            this.editInterfaceView.Size = new System.Drawing.Size(193, 467);
            this.editInterfaceView.TabIndex = 0;
            // 
            // SimObjectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editInterfaceView);
            this.Name = "SimObjectPanel";
            this.Size = new System.Drawing.Size(193, 467);
            this.ResumeLayout(false);

        }

        #endregion

        private Editor.EditInterfaceView editInterfaceView;


    }
}
