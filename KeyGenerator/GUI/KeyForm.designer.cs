namespace KeyGenerator
{
    partial class KeyForm
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
            this.generateKey = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.keyText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.programNameText = new System.Windows.Forms.TextBox();
            this.openButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // generateKey
            // 
            this.generateKey.Location = new System.Drawing.Point(12, 107);
            this.generateKey.Name = "generateKey";
            this.generateKey.Size = new System.Drawing.Size(154, 35);
            this.generateKey.TabIndex = 0;
            this.generateKey.Text = "Generate Key";
            this.generateKey.UseVisualStyleBackColor = true;
            this.generateKey.Click += new System.EventHandler(this.generateKey_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Key";
            // 
            // keyText
            // 
            this.keyText.Location = new System.Drawing.Point(12, 81);
            this.keyText.Name = "keyText";
            this.keyText.Size = new System.Drawing.Size(376, 20);
            this.keyText.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Program Name";
            // 
            // programNameText
            // 
            this.programNameText.Location = new System.Drawing.Point(12, 26);
            this.programNameText.Name = "programNameText";
            this.programNameText.Size = new System.Drawing.Size(266, 20);
            this.programNameText.TabIndex = 4;
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(283, 19);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(104, 34);
            this.openButton.TabIndex = 5;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // KeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 154);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.programNameText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.keyText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.generateKey);
            this.Name = "KeyForm";
            this.Text = "Anomalous Medical Key Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox keyText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox programNameText;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

