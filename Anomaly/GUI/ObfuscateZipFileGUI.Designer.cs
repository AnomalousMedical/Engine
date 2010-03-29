namespace Anomaly
{
    partial class ObfuscateZipFileGUI
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.destTextBox = new System.Windows.Forms.TextBox();
            this.browseSourceFileButton = new System.Windows.Forms.Button();
            this.browseDestFileButton = new System.Windows.Forms.Button();
            this.obfuscateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source File";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination File";
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceTextBox.Location = new System.Drawing.Point(8, 23);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(322, 20);
            this.sourceTextBox.TabIndex = 2;
            // 
            // destTextBox
            // 
            this.destTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.destTextBox.Location = new System.Drawing.Point(8, 64);
            this.destTextBox.Name = "destTextBox";
            this.destTextBox.Size = new System.Drawing.Size(322, 20);
            this.destTextBox.TabIndex = 3;
            // 
            // browseSourceFileButton
            // 
            this.browseSourceFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseSourceFileButton.Location = new System.Drawing.Point(336, 21);
            this.browseSourceFileButton.Name = "browseSourceFileButton";
            this.browseSourceFileButton.Size = new System.Drawing.Size(36, 23);
            this.browseSourceFileButton.TabIndex = 4;
            this.browseSourceFileButton.Text = "...";
            this.browseSourceFileButton.UseVisualStyleBackColor = true;
            this.browseSourceFileButton.Click += new System.EventHandler(this.browseSourceFileButton_Click);
            // 
            // browseDestFileButton
            // 
            this.browseDestFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseDestFileButton.Location = new System.Drawing.Point(336, 62);
            this.browseDestFileButton.Name = "browseDestFileButton";
            this.browseDestFileButton.Size = new System.Drawing.Size(36, 23);
            this.browseDestFileButton.TabIndex = 5;
            this.browseDestFileButton.Text = "...";
            this.browseDestFileButton.UseVisualStyleBackColor = true;
            this.browseDestFileButton.Click += new System.EventHandler(this.browseDestFileButton_Click);
            // 
            // obfuscateButton
            // 
            this.obfuscateButton.Location = new System.Drawing.Point(8, 91);
            this.obfuscateButton.Name = "obfuscateButton";
            this.obfuscateButton.Size = new System.Drawing.Size(75, 23);
            this.obfuscateButton.TabIndex = 6;
            this.obfuscateButton.Text = "Obfuscate";
            this.obfuscateButton.UseVisualStyleBackColor = true;
            this.obfuscateButton.Click += new System.EventHandler(this.obfuscateButton_Click);
            // 
            // ObfuscateZipFileGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 120);
            this.Controls.Add(this.obfuscateButton);
            this.Controls.Add(this.browseDestFileButton);
            this.Controls.Add(this.browseSourceFileButton);
            this.Controls.Add(this.destTextBox);
            this.Controls.Add(this.sourceTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ObfuscateZipFileGUI";
            this.Text = "Obfuscate Archive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.TextBox destTextBox;
        private System.Windows.Forms.Button browseSourceFileButton;
        private System.Windows.Forms.Button browseDestFileButton;
        private System.Windows.Forms.Button obfuscateButton;
    }
}