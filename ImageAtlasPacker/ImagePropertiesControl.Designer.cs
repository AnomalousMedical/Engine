namespace ImageAtlasPacker
{
    partial class ImagePropertiesControl
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
            this.components = new System.ComponentModel.Container();
            this.inputTextureList = new System.Windows.Forms.ListView();
            this.previewImageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.previewImageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.widthText = new System.Windows.Forms.NumericUpDown();
            this.heightText = new System.Windows.Forms.NumericUpDown();
            this.updateButton = new System.Windows.Forms.Button();
            this.addTexturesButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.removeTextures = new System.Windows.Forms.Button();
            this.displayChangeButton = new System.Windows.Forms.Button();
            this.resizeImages = new System.Windows.Forms.CheckBox();
            this.resizeHeight = new System.Windows.Forms.NumericUpDown();
            this.resizeWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.halveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.widthText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resizeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resizeWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // inputTextureList
            // 
            this.inputTextureList.AllowDrop = true;
            this.inputTextureList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextureList.LargeImageList = this.previewImageListLarge;
            this.inputTextureList.Location = new System.Drawing.Point(3, 3);
            this.inputTextureList.Name = "inputTextureList";
            this.inputTextureList.Size = new System.Drawing.Size(244, 387);
            this.inputTextureList.SmallImageList = this.previewImageListSmall;
            this.inputTextureList.TabIndex = 0;
            this.inputTextureList.UseCompatibleStateImageBehavior = false;
            // 
            // previewImageListLarge
            // 
            this.previewImageListLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.previewImageListLarge.ImageSize = new System.Drawing.Size(100, 100);
            this.previewImageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // previewImageListSmall
            // 
            this.previewImageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.previewImageListSmall.ImageSize = new System.Drawing.Size(32, 32);
            this.previewImageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 422);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Texture Width";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 462);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Texture Height";
            // 
            // widthText
            // 
            this.widthText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.widthText.Location = new System.Drawing.Point(10, 439);
            this.widthText.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.widthText.Name = "widthText";
            this.widthText.Size = new System.Drawing.Size(120, 20);
            this.widthText.TabIndex = 3;
            this.widthText.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // heightText
            // 
            this.heightText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.heightText.Location = new System.Drawing.Point(10, 479);
            this.heightText.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.heightText.Name = "heightText";
            this.heightText.Size = new System.Drawing.Size(120, 20);
            this.heightText.TabIndex = 4;
            this.heightText.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.updateButton.Location = new System.Drawing.Point(3, 505);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 5;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // addTexturesButton
            // 
            this.addTexturesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addTexturesButton.Location = new System.Drawing.Point(4, 393);
            this.addTexturesButton.Name = "addTexturesButton";
            this.addTexturesButton.Size = new System.Drawing.Size(37, 23);
            this.addTexturesButton.TabIndex = 6;
            this.addTexturesButton.Text = "Add";
            this.addTexturesButton.UseVisualStyleBackColor = true;
            this.addTexturesButton.Click += new System.EventHandler(this.addTexturesButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            // 
            // removeTextures
            // 
            this.removeTextures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeTextures.Location = new System.Drawing.Point(45, 393);
            this.removeTextures.Name = "removeTextures";
            this.removeTextures.Size = new System.Drawing.Size(56, 23);
            this.removeTextures.TabIndex = 7;
            this.removeTextures.Text = "Remove";
            this.removeTextures.UseVisualStyleBackColor = true;
            this.removeTextures.Click += new System.EventHandler(this.removeTextures_Click);
            // 
            // displayChangeButton
            // 
            this.displayChangeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayChangeButton.Location = new System.Drawing.Point(107, 393);
            this.displayChangeButton.Name = "displayChangeButton";
            this.displayChangeButton.Size = new System.Drawing.Size(49, 23);
            this.displayChangeButton.TabIndex = 8;
            this.displayChangeButton.Text = "Display";
            this.displayChangeButton.UseVisualStyleBackColor = true;
            this.displayChangeButton.Click += new System.EventHandler(this.displayChangeButton_Click);
            // 
            // resizeImages
            // 
            this.resizeImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resizeImages.AutoSize = true;
            this.resizeImages.Location = new System.Drawing.Point(147, 424);
            this.resizeImages.Name = "resizeImages";
            this.resizeImages.Size = new System.Drawing.Size(95, 17);
            this.resizeImages.TabIndex = 9;
            this.resizeImages.Text = "Resize Images";
            this.resizeImages.UseVisualStyleBackColor = true;
            // 
            // resizeHeight
            // 
            this.resizeHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resizeHeight.Location = new System.Drawing.Point(147, 503);
            this.resizeHeight.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.resizeHeight.Name = "resizeHeight";
            this.resizeHeight.Size = new System.Drawing.Size(90, 20);
            this.resizeHeight.TabIndex = 13;
            this.resizeHeight.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // resizeWidth
            // 
            this.resizeWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resizeWidth.Location = new System.Drawing.Point(147, 463);
            this.resizeWidth.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.resizeWidth.Name = "resizeWidth";
            this.resizeWidth.Size = new System.Drawing.Size(90, 20);
            this.resizeWidth.TabIndex = 12;
            this.resizeWidth.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 486);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Resize Height";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 446);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Resize Width";
            // 
            // halveButton
            // 
            this.halveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.halveButton.Location = new System.Drawing.Point(81, 505);
            this.halveButton.Name = "halveButton";
            this.halveButton.Size = new System.Drawing.Size(49, 23);
            this.halveButton.TabIndex = 14;
            this.halveButton.Text = "Halve";
            this.halveButton.UseVisualStyleBackColor = true;
            this.halveButton.Click += new System.EventHandler(this.halveButton_Click);
            // 
            // ImagePropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.halveButton);
            this.Controls.Add(this.resizeHeight);
            this.Controls.Add(this.resizeWidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.resizeImages);
            this.Controls.Add(this.displayChangeButton);
            this.Controls.Add(this.removeTextures);
            this.Controls.Add(this.addTexturesButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.heightText);
            this.Controls.Add(this.widthText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputTextureList);
            this.Name = "ImagePropertiesControl";
            this.Size = new System.Drawing.Size(251, 533);
            ((System.ComponentModel.ISupportInitialize)(this.widthText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resizeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resizeWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView inputTextureList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown widthText;
        private System.Windows.Forms.NumericUpDown heightText;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.ImageList previewImageListSmall;
        private System.Windows.Forms.Button addTexturesButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ImageList previewImageListLarge;
        private System.Windows.Forms.Button removeTextures;
        private System.Windows.Forms.Button displayChangeButton;
        private System.Windows.Forms.CheckBox resizeImages;
        private System.Windows.Forms.NumericUpDown resizeHeight;
        private System.Windows.Forms.NumericUpDown resizeWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button halveButton;
    }
}
