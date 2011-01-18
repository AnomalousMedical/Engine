using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Anomaly.GUI
{
    public partial class ProjectSelector : Form
    {
        public ProjectSelector()
        {
            InitializeComponent();
            recentDocsList.SelectedIndexChanged += new EventHandler(recentDocsList_SelectedIndexChanged);
            recentDocsList.MouseDoubleClick += new MouseEventHandler(recentDocsList_MouseDoubleClick);
        }

        public void setRecentFiles(RecentDocuments recentDocs)
        {
            recentDocsList.Clear();
            foreach(String doc in recentDocs)
            {
                ListViewItem item = new ListViewItem();
                item.Text = Path.GetFileNameWithoutExtension(doc);
                item.Tag = doc;
                item.ImageKey = "ProjectIcon";
                recentDocsList.Items.Add(item);
            }
        }

        public String SelectedFile
        {
            get
            {
                return selectedFileText.Text;
            }
            set
            {
                selectedFileText.Text = value;
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                selectedFileText.Text = openFileDialog1.FileName;
            }
        }

        void recentDocsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recentDocsList.SelectedItems.Count > 0)
            {
                selectedFileText.Text = recentDocsList.SelectedItems[0].Tag.ToString();
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        void recentDocsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openButton_Click(null, null);
        }
    }
}
