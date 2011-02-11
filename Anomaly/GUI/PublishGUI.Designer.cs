namespace Anomaly
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
            this.components = new System.ComponentModel.Container();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.recursiveCheckBox = new System.Windows.Forms.CheckBox();
            this.addFileButton = new System.Windows.Forms.Button();
            this.addDirectoryButton = new System.Windows.Forms.Button();
            this.saveResourceProfile = new System.Windows.Forms.Button();
            this.resourceProfileCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.archiveNameText = new System.Windows.Forms.TextBox();
            this.obfuscateCheckBox = new System.Windows.Forms.CheckBox();
            this.archiveCheckBox = new System.Windows.Forms.CheckBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.outputLocationTextBox = new System.Windows.Forms.TextBox();
            this.publishButton = new System.Windows.Forms.Button();
            this.fileView = new System.Windows.Forms.ListView();
            this.fileNameColumn = new System.Windows.Forms.ColumnHeader();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ignoreDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonPanel.SuspendLayout();
            this.groupMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.recursiveCheckBox);
            this.buttonPanel.Controls.Add(this.addFileButton);
            this.buttonPanel.Controls.Add(this.addDirectoryButton);
            this.buttonPanel.Controls.Add(this.saveResourceProfile);
            this.buttonPanel.Controls.Add(this.resourceProfileCombo);
            this.buttonPanel.Controls.Add(this.label2);
            this.buttonPanel.Controls.Add(this.archiveNameText);
            this.buttonPanel.Controls.Add(this.obfuscateCheckBox);
            this.buttonPanel.Controls.Add(this.archiveCheckBox);
            this.buttonPanel.Controls.Add(this.browseButton);
            this.buttonPanel.Controls.Add(this.label1);
            this.buttonPanel.Controls.Add(this.outputLocationTextBox);
            this.buttonPanel.Controls.Add(this.publishButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 330);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(526, 167);
            this.buttonPanel.TabIndex = 0;
            // 
            // recursiveCheckBox
            // 
            this.recursiveCheckBox.AutoSize = true;
            this.recursiveCheckBox.Location = new System.Drawing.Point(180, 9);
            this.recursiveCheckBox.Name = "recursiveCheckBox";
            this.recursiveCheckBox.Size = new System.Drawing.Size(74, 17);
            this.recursiveCheckBox.TabIndex = 13;
            this.recursiveCheckBox.Text = "Recursive";
            this.recursiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // addFileButton
            // 
            this.addFileButton.Location = new System.Drawing.Point(108, 5);
            this.addFileButton.Name = "addFileButton";
            this.addFileButton.Size = new System.Drawing.Size(64, 24);
            this.addFileButton.TabIndex = 12;
            this.addFileButton.Text = "Add File";
            this.addFileButton.UseVisualStyleBackColor = true;
            this.addFileButton.Click += new System.EventHandler(this.addFileButton_Click);
            // 
            // addDirectoryButton
            // 
            this.addDirectoryButton.Location = new System.Drawing.Point(6, 5);
            this.addDirectoryButton.Name = "addDirectoryButton";
            this.addDirectoryButton.Size = new System.Drawing.Size(97, 24);
            this.addDirectoryButton.TabIndex = 11;
            this.addDirectoryButton.Text = "Add Directory";
            this.addDirectoryButton.UseVisualStyleBackColor = true;
            this.addDirectoryButton.Click += new System.EventHandler(this.addDirectoryButton_Click);
            // 
            // saveResourceProfile
            // 
            this.saveResourceProfile.Location = new System.Drawing.Point(366, 33);
            this.saveResourceProfile.Name = "saveResourceProfile";
            this.saveResourceProfile.Size = new System.Drawing.Size(75, 23);
            this.saveResourceProfile.TabIndex = 10;
            this.saveResourceProfile.Text = "Save";
            this.saveResourceProfile.UseVisualStyleBackColor = true;
            this.saveResourceProfile.Click += new System.EventHandler(this.saveResourceProfile_Click);
            // 
            // resourceProfileCombo
            // 
            this.resourceProfileCombo.FormattingEnabled = true;
            this.resourceProfileCombo.Location = new System.Drawing.Point(94, 34);
            this.resourceProfileCombo.Name = "resourceProfileCombo";
            this.resourceProfileCombo.Size = new System.Drawing.Size(266, 21);
            this.resourceProfileCombo.TabIndex = 9;
            this.resourceProfileCombo.SelectedIndexChanged += new System.EventHandler(this.resourceProfileCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Resource Profile";
            // 
            // archiveNameText
            // 
            this.archiveNameText.Location = new System.Drawing.Point(109, 91);
            this.archiveNameText.Name = "archiveNameText";
            this.archiveNameText.Size = new System.Drawing.Size(252, 20);
            this.archiveNameText.TabIndex = 6;
            // 
            // obfuscateCheckBox
            // 
            this.obfuscateCheckBox.AutoSize = true;
            this.obfuscateCheckBox.Checked = true;
            this.obfuscateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.obfuscateCheckBox.Location = new System.Drawing.Point(7, 117);
            this.obfuscateCheckBox.Name = "obfuscateCheckBox";
            this.obfuscateCheckBox.Size = new System.Drawing.Size(75, 17);
            this.obfuscateCheckBox.TabIndex = 5;
            this.obfuscateCheckBox.Text = "Obfuscate";
            this.obfuscateCheckBox.UseVisualStyleBackColor = true;
            // 
            // archiveCheckBox
            // 
            this.archiveCheckBox.AutoSize = true;
            this.archiveCheckBox.Checked = true;
            this.archiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.archiveCheckBox.Location = new System.Drawing.Point(7, 94);
            this.archiveCheckBox.Name = "archiveCheckBox";
            this.archiveCheckBox.Size = new System.Drawing.Size(96, 17);
            this.archiveCheckBox.TabIndex = 4;
            this.archiveCheckBox.Text = "Create Archive";
            this.archiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(367, 60);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 3;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Output Directory";
            // 
            // outputLocationTextBox
            // 
            this.outputLocationTextBox.Location = new System.Drawing.Point(94, 62);
            this.outputLocationTextBox.Name = "outputLocationTextBox";
            this.outputLocationTextBox.Size = new System.Drawing.Size(267, 20);
            this.outputLocationTextBox.TabIndex = 1;
            // 
            // publishButton
            // 
            this.publishButton.Location = new System.Drawing.Point(7, 140);
            this.publishButton.Name = "publishButton";
            this.publishButton.Size = new System.Drawing.Size(75, 23);
            this.publishButton.TabIndex = 0;
            this.publishButton.Text = "Publish";
            this.publishButton.UseVisualStyleBackColor = true;
            this.publishButton.Click += new System.EventHandler(this.publishButton_Click);
            // 
            // fileView
            // 
            this.fileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNameColumn});
            this.fileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileView.Location = new System.Drawing.Point(0, 0);
            this.fileView.Name = "fileView";
            this.fileView.Size = new System.Drawing.Size(526, 330);
            this.fileView.TabIndex = 1;
            this.fileView.UseCompatibleStateImageBehavior = false;
            this.fileView.View = System.Windows.Forms.View.Details;
            // 
            // fileNameColumn
            // 
            this.fileNameColumn.Text = "Name";
            // 
            // groupMenuStrip
            // 
            this.groupMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignoreDirectoryToolStripMenuItem});
            this.groupMenuStrip.Name = "groupMenuStrip";
            this.groupMenuStrip.Size = new System.Drawing.Size(160, 26);
            // 
            // ignoreDirectoryToolStripMenuItem
            // 
            this.ignoreDirectoryToolStripMenuItem.Name = "ignoreDirectoryToolStripMenuItem";
            this.ignoreDirectoryToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.ignoreDirectoryToolStripMenuItem.Text = "Ignore Directory";
            this.ignoreDirectoryToolStripMenuItem.Click += new System.EventHandler(this.ignoreDirectoryToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // PublishGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 497);
            this.Controls.Add(this.fileView);
            this.Controls.Add(this.buttonPanel);
            this.Name = "PublishGUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publish";
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.groupMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox archiveNameText;
        private System.Windows.Forms.CheckBox obfuscateCheckBox;
        private System.Windows.Forms.CheckBox archiveCheckBox;
        private System.Windows.Forms.ContextMenuStrip groupMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ignoreDirectoryToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox resourceProfileCombo;
        private System.Windows.Forms.Button saveResourceProfile;
        private System.Windows.Forms.Button addDirectoryButton;
        private System.Windows.Forms.Button addFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox recursiveCheckBox;
    }
}