namespace OgreModelEditor
{
    partial class CustomParameterControl
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
            this.setValue = new System.Windows.Forms.Button();
            this.getValue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.indexUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.valueText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.materialText = new System.Windows.Forms.TextBox();
            this.applyMaterialButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.indexUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // setValue
            // 
            this.setValue.Location = new System.Drawing.Point(7, 65);
            this.setValue.Name = "setValue";
            this.setValue.Size = new System.Drawing.Size(74, 23);
            this.setValue.TabIndex = 0;
            this.setValue.Text = "Set Value";
            this.setValue.UseVisualStyleBackColor = true;
            this.setValue.Click += new System.EventHandler(this.setValue_Click);
            // 
            // getValue
            // 
            this.getValue.Location = new System.Drawing.Point(87, 65);
            this.getValue.Name = "getValue";
            this.getValue.Size = new System.Drawing.Size(74, 23);
            this.getValue.TabIndex = 1;
            this.getValue.Text = "Get Value";
            this.getValue.UseVisualStyleBackColor = true;
            this.getValue.Click += new System.EventHandler(this.getValue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Index";
            // 
            // indexUpDown
            // 
            this.indexUpDown.Location = new System.Drawing.Point(6, 37);
            this.indexUpDown.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.indexUpDown.Name = "indexUpDown";
            this.indexUpDown.Size = new System.Drawing.Size(60, 20);
            this.indexUpDown.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value";
            // 
            // valueText
            // 
            this.valueText.Location = new System.Drawing.Point(75, 37);
            this.valueText.Name = "valueText";
            this.valueText.Size = new System.Drawing.Size(204, 20);
            this.valueText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Custom Parameter";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Material";
            // 
            // materialText
            // 
            this.materialText.Location = new System.Drawing.Point(8, 111);
            this.materialText.Name = "materialText";
            this.materialText.Size = new System.Drawing.Size(262, 20);
            this.materialText.TabIndex = 8;
            // 
            // applyMaterialButton
            // 
            this.applyMaterialButton.Location = new System.Drawing.Point(8, 138);
            this.applyMaterialButton.Name = "applyMaterialButton";
            this.applyMaterialButton.Size = new System.Drawing.Size(95, 23);
            this.applyMaterialButton.TabIndex = 9;
            this.applyMaterialButton.Text = "Apply Material";
            this.applyMaterialButton.UseVisualStyleBackColor = true;
            this.applyMaterialButton.Click += new System.EventHandler(this.applyMaterialButton_Click);
            // 
            // CustomParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 169);
            this.Controls.Add(this.applyMaterialButton);
            this.Controls.Add(this.materialText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.valueText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.indexUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.getValue);
            this.Controls.Add(this.setValue);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "CustomParameterControl";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.Text = "Parameters";
            ((System.ComponentModel.ISupportInitialize)(this.indexUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setValue;
        private System.Windows.Forms.Button getValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown indexUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox valueText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox materialText;
        private System.Windows.Forms.Button applyMaterialButton;
    }
}
