﻿namespace Anomaly
{
    partial class PublishGUI
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
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.fileView = new System.Windows.Forms.ListView();
            this.fileNameColumn = new System.Windows.Forms.ColumnHeader();
            this.publishButton = new System.Windows.Forms.Button();
            this.outputLocationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.browseButton);
            this.buttonPanel.Controls.Add(this.label1);
            this.buttonPanel.Controls.Add(this.outputLocationTextBox);
            this.buttonPanel.Controls.Add(this.publishButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 398);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(526, 34);
            this.buttonPanel.TabIndex = 0;
            // 
            // fileView
            // 
            this.fileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNameColumn});
            this.fileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileView.Location = new System.Drawing.Point(0, 0);
            this.fileView.Name = "fileView";
            this.fileView.Size = new System.Drawing.Size(526, 398);
            this.fileView.TabIndex = 1;
            this.fileView.UseCompatibleStateImageBehavior = false;
            this.fileView.View = System.Windows.Forms.View.Details;
            // 
            // fileNameColumn
            // 
            this.fileNameColumn.Text = "Name";
            // 
            // publishButton
            // 
            this.publishButton.Location = new System.Drawing.Point(448, 6);
            this.publishButton.Name = "publishButton";
            this.publishButton.Size = new System.Drawing.Size(75, 23);
            this.publishButton.TabIndex = 0;
            this.publishButton.Text = "Publish";
            this.publishButton.UseVisualStyleBackColor = true;
            this.publishButton.Click += new System.EventHandler(this.publishButton_Click);
            // 
            // outputLocationTextBox
            // 
            this.outputLocationTextBox.Location = new System.Drawing.Point(94, 8);
            this.outputLocationTextBox.Name = "outputLocationTextBox";
            this.outputLocationTextBox.Size = new System.Drawing.Size(267, 20);
            this.outputLocationTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Output Directory";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(367, 6);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 3;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // PublishGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 432);
            this.Controls.Add(this.fileView);
            this.Controls.Add(this.buttonPanel);
            this.Name = "PublishGUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publish";
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.ListView fileView;
        private System.Windows.Forms.ColumnHeader fileNameColumn;
        private System.Windows.Forms.Button publishButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputLocationTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}