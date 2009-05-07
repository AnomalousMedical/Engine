using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    public partial class TwoWaySplit : UserControl, SplitView
    {
        public TwoWaySplit()
        {
            InitializeComponent();
        }

        public Control FrontView
        {
            get
            {
                return verticalSplit.Panel1;
            }
        }

        public Control BackView
        {
            get
            {
                return verticalSplit.Panel2;
            }
        }

        public Control LeftView
        {
            get
            {
                return null;
            }
        }

        public Control RightView
        {
            get
            {
                return null;
            }
        }
    }
}
