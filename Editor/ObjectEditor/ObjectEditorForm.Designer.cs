namespace Editor
{
    partial class ObjectEditorForm
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
            this.objectEditorPanel = new Editor.ObjectEditorPanel();
            this.SuspendLayout();
            // 
            // objectEditorPanel
            // 
            this.objectEditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectEditorPanel.Location = new System.Drawing.Point(0, 0);
            this.objectEditorPanel.Name = "objectEditorPanel";
            this.objectEditorPanel.Size = new System.Drawing.Size(750, 444);
            this.objectEditorPanel.TabIndex = 0;
            // 
            // ObjectEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 444);
            this.Controls.Add(this.objectEditorPanel);
            this.Name = "ObjectEditorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ObjectEditorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ObjectEditorPanel objectEditorPanel;
    }
}