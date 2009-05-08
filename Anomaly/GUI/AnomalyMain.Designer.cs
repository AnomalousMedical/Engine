﻿namespace Anomaly
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnomalyMain));
            this.mainToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.eulerRotatePanel1 = new Editor.EulerRotatePanel();
            this.movePanel = new Editor.MovePanel();
            this.objectViewSplit = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.simObjectPanel = new Anomaly.SimObjectPanel();
            this.objectsEditInterface = new Editor.EditInterfaceView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.templatePanel1 = new Anomaly.TemplatePanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.playButton = new System.Windows.Forms.ToolStripButton();
            this.pauseButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.rotateButton = new System.Windows.Forms.ToolStripButton();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twoWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threeWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fourWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maximizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripContainer.ContentPanel.SuspendLayout();
            this.mainToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.mainToolStripContainer.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.objectViewSplit.Panel1.SuspendLayout();
            this.objectViewSplit.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolStripContainer
            // 
            // 
            // mainToolStripContainer.ContentPanel
            // 
            this.mainToolStripContainer.ContentPanel.Controls.Add(this.toolPanel);
            this.mainToolStripContainer.ContentPanel.Controls.Add(this.objectViewSplit);
            this.mainToolStripContainer.ContentPanel.Size = new System.Drawing.Size(752, 491);
            this.mainToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainToolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.mainToolStripContainer.Name = "mainToolStripContainer";
            this.mainToolStripContainer.Size = new System.Drawing.Size(752, 516);
            this.mainToolStripContainer.TabIndex = 3;
            this.mainToolStripContainer.Text = "toolStripContainer1";
            // 
            // mainToolStripContainer.TopToolStripPanel
            // 
            this.mainToolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.mainToolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolPanel
            // 
            this.toolPanel.Controls.Add(this.eulerRotatePanel1);
            this.toolPanel.Controls.Add(this.movePanel);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolPanel.Location = new System.Drawing.Point(0, 434);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(752, 57);
            this.toolPanel.TabIndex = 7;
            // 
            // eulerRotatePanel1
            // 
            this.eulerRotatePanel1.Location = new System.Drawing.Point(252, 8);
            this.eulerRotatePanel1.Name = "eulerRotatePanel1";
            this.eulerRotatePanel1.Size = new System.Drawing.Size(340, 46);
            this.eulerRotatePanel1.TabIndex = 1;
            // 
            // movePanel
            // 
            this.movePanel.Location = new System.Drawing.Point(3, 3);
            this.movePanel.Name = "movePanel";
            this.movePanel.Size = new System.Drawing.Size(243, 52);
            this.movePanel.TabIndex = 0;
            // 
            // objectViewSplit
            // 
            this.objectViewSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectViewSplit.Location = new System.Drawing.Point(0, 0);
            this.objectViewSplit.Name = "objectViewSplit";
            // 
            // objectViewSplit.Panel1
            // 
            this.objectViewSplit.Panel1.Controls.Add(this.tabControl1);
            this.objectViewSplit.Size = new System.Drawing.Size(752, 406);
            this.objectViewSplit.SplitterDistance = 162;
            this.objectViewSplit.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(162, 406);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.simObjectPanel);
            this.tabPage1.Controls.Add(this.objectsEditInterface);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(154, 380);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Objects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // simObjectPanel
            // 
            this.simObjectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simObjectPanel.Location = new System.Drawing.Point(3, 3);
            this.simObjectPanel.Name = "simObjectPanel";
            this.simObjectPanel.Size = new System.Drawing.Size(148, 374);
            this.simObjectPanel.TabIndex = 1;
            // 
            // objectsEditInterface
            // 
            this.objectsEditInterface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsEditInterface.Location = new System.Drawing.Point(3, 3);
            this.objectsEditInterface.Name = "objectsEditInterface";
            this.objectsEditInterface.Size = new System.Drawing.Size(148, 374);
            this.objectsEditInterface.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.templatePanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(154, 380);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Templates";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // templatePanel1
            // 
            this.templatePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templatePanel1.Location = new System.Drawing.Point(3, 3);
            this.templatePanel1.Name = "templatePanel1";
            this.templatePanel1.Size = new System.Drawing.Size(148, 374);
            this.templatePanel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playButton,
            this.pauseButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(87, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // playButton
            // 
            this.playButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.playButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
            this.playButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(33, 22);
            this.playButton.Text = "Play";
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.pauseButton.Enabled = false;
            this.pauseButton.Image = ((System.Drawing.Image)(resources.GetObject("pauseButton.Image")));
            this.pauseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(42, 22);
            this.pauseButton.Text = "Pause";
            this.pauseButton.ToolTipText = "Pause";
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveButton,
            this.rotateButton});
            this.toolStrip2.Location = new System.Drawing.Point(90, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(98, 25);
            this.toolStrip2.TabIndex = 1;
            // 
            // moveButton
            // 
            this.moveButton.Checked = true;
            this.moveButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.moveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.moveButton.Image = ((System.Drawing.Image)(resources.GetObject("moveButton.Image")));
            this.moveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(41, 22);
            this.moveButton.Text = "Move";
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // rotateButton
            // 
            this.rotateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.rotateButton.Image = ((System.Drawing.Image)(resources.GetObject("rotateButton.Image")));
            this.rotateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateButton.Name = "rotateButton";
            this.rotateButton.Size = new System.Drawing.Size(45, 22);
            this.rotateButton.Text = "Rotate";
            this.rotateButton.Click += new System.EventHandler(this.rotateButton_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.resourcesToolStripMenuItem,
            this.sceneToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(752, 24);
            this.mainMenu.TabIndex = 4;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // resourcesToolStripMenuItem
            // 
            this.resourcesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.resourcesToolStripMenuItem.Name = "resourcesToolStripMenuItem";
            this.resourcesToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.resourcesToolStripMenuItem.Text = "Resources";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // sceneToolStripMenuItem
            // 
            this.sceneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem});
            this.sceneToolStripMenuItem.Name = "sceneToolStripMenuItem";
            this.sceneToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.sceneToolStripMenuItem.Text = "Scene";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showStatsToolStripMenuItem,
            this.maximizeToolStripMenuItem,
            this.layoutToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // showStatsToolStripMenuItem
            // 
            this.showStatsToolStripMenuItem.Name = "showStatsToolStripMenuItem";
            this.showStatsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showStatsToolStripMenuItem.Text = "Show Stats";
            this.showStatsToolStripMenuItem.Click += new System.EventHandler(this.showStatsToolStripMenuItem_Click);
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oneWindowToolStripMenuItem,
            this.twoWindowsToolStripMenuItem,
            this.threeWindowsToolStripMenuItem,
            this.fourWindowsToolStripMenuItem});
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.layoutToolStripMenuItem.Text = "Layout";
            // 
            // oneWindowToolStripMenuItem
            // 
            this.oneWindowToolStripMenuItem.Name = "oneWindowToolStripMenuItem";
            this.oneWindowToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.oneWindowToolStripMenuItem.Text = "One Window";
            this.oneWindowToolStripMenuItem.Click += new System.EventHandler(this.oneWindowToolStripMenuItem_Click);
            // 
            // twoWindowsToolStripMenuItem
            // 
            this.twoWindowsToolStripMenuItem.Name = "twoWindowsToolStripMenuItem";
            this.twoWindowsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.twoWindowsToolStripMenuItem.Text = "Two Windows";
            this.twoWindowsToolStripMenuItem.Click += new System.EventHandler(this.twoWindowsToolStripMenuItem_Click);
            // 
            // threeWindowsToolStripMenuItem
            // 
            this.threeWindowsToolStripMenuItem.Name = "threeWindowsToolStripMenuItem";
            this.threeWindowsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.threeWindowsToolStripMenuItem.Text = "Three Windows";
            this.threeWindowsToolStripMenuItem.Click += new System.EventHandler(this.threeWindowsToolStripMenuItem_Click);
            // 
            // fourWindowsToolStripMenuItem
            // 
            this.fourWindowsToolStripMenuItem.Name = "fourWindowsToolStripMenuItem";
            this.fourWindowsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.fourWindowsToolStripMenuItem.Text = "Four Windows";
            this.fourWindowsToolStripMenuItem.Click += new System.EventHandler(this.fourWindowsToolStripMenuItem_Click);
            // 
            // maximizeToolStripMenuItem
            // 
            this.maximizeToolStripMenuItem.Name = "maximizeToolStripMenuItem";
            this.maximizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.maximizeToolStripMenuItem.Text = "Maximize";
            this.maximizeToolStripMenuItem.Click += new System.EventHandler(this.maximizeToolStripMenuItem_Click);
            // 
            // AnomalyMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 540);
            this.Controls.Add(this.mainToolStripContainer);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "AnomalyMain";
            this.Text = "Anomaly";
            this.mainToolStripContainer.ContentPanel.ResumeLayout(false);
            this.mainToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.mainToolStripContainer.TopToolStripPanel.PerformLayout();
            this.mainToolStripContainer.ResumeLayout(false);
            this.mainToolStripContainer.PerformLayout();
            this.toolPanel.ResumeLayout(false);
            this.objectViewSplit.Panel1.ResumeLayout(false);
            this.objectViewSplit.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer mainToolStripContainer;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel toolPanel;
        private Editor.MovePanel movePanel;
        private System.Windows.Forms.SplitContainer objectViewSplit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Editor.EditInterfaceView objectsEditInterface;
        private System.Windows.Forms.TabPage tabPage2;
        private TemplatePanel templatePanel1;
        private System.Windows.Forms.ToolStripMenuItem resourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private SimObjectPanel simObjectPanel;
        private Editor.EulerRotatePanel eulerRotatePanel1;
        private System.Windows.Forms.ToolStripMenuItem sceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton playButton;
        private System.Windows.Forms.ToolStripButton pauseButton;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton moveButton;
        private System.Windows.Forms.ToolStripButton rotateButton;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showStatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twoWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threeWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fourWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maximizeToolStripMenuItem;



    }
}

