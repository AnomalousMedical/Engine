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
    public partial class OneWaySplit : UserControl, SplitView
    {
        public OneWaySplit()
        {
            InitializeComponent();
        }

        #region SplitView Members

        public Control FrontView
        {
            get
            {
                return this;
            }
        }

        public Control BackView
        {
            get
            {
                return null;
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

        #endregion
    }
}
