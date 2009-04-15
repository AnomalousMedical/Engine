namespace Editor
{
    partial class MultiEnumEditorCellPopup
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
            this.multiEnumEditor = new Editor.MultiEnumEditor();
            this.SuspendLayout();
            // 
            // multiEnumEditor
            // 
            this.multiEnumEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiEnumEditor.Location = new System.Drawing.Point(0, 0);
            this.multiEnumEditor.Name = "multiEnumEditor";
            this.multiEnumEditor.Size = new System.Drawing.Size(482, 381);
            this.multiEnumEditor.TabIndex = 0;
            this.multiEnumEditor.Value = ((long)(0));
            // 
            // MultiEnumEditorCellPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 381);
            this.Controls.Add(this.multiEnumEditor);
            this.Name = "MultiEnumEditorCellPopup";
            this.Text = "MultiEnumEditorCellPopup";
            this.ResumeLayout(false);

        }

        #endregion

        private MultiEnumEditor multiEnumEditor;
    }
}