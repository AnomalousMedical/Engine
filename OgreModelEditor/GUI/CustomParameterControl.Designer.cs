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
            ((System.ComponentModel.ISupportInitialize)(this.indexUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // setValue
            // 
            this.setValue.Location = new System.Drawing.Point(8, 51);
            this.setValue.Name = "setValue";
            this.setValue.Size = new System.Drawing.Size(74, 23);
            this.setValue.TabIndex = 0;
            this.setValue.Text = "Set Value";
            this.setValue.UseVisualStyleBackColor = true;
            this.setValue.Click += new System.EventHandler(this.setValue_Click);
            // 
            // getValue
            // 
            this.getValue.Location = new System.Drawing.Point(88, 51);
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
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Index";
            // 
            // indexUpDown
            // 
            this.indexUpDown.Location = new System.Drawing.Point(7, 23);
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
            this.label2.Location = new System.Drawing.Point(72, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value";
            // 
            // valueText
            // 
            this.valueText.Location = new System.Drawing.Point(76, 23);
            this.valueText.Name = "valueText";
            this.valueText.Size = new System.Drawing.Size(204, 20);
            this.valueText.TabIndex = 5;
            // 
            // CustomParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 78);
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
            this.Text = "Custom Parameters";
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
    }
}
