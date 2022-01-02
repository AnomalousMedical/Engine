using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageAtlasPacker
{
    public partial class ImageIndexControl : UserControl
    {
        public ImageIndexControl()
        {
            InitializeComponent();
        }

        public String HeaderText
        {
            get
            {
                return headerTextBox.Text;
            }
            set
            {
                headerTextBox.Text = value;
            }
        }

        public String FooterText
        {
            get
            {
                return footerTextBox.Text;
            }
            set
            {
                footerTextBox.Text = value;
            }
        }

        public String IndexText
        {
            get
            {
                return indexTextBox.Text;
            }
            set
            {
                indexTextBox.Text = value;
            }
        }
    }
}
