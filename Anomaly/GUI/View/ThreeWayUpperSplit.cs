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
    public partial class ThreeWayUpperSplit : UserControl, SplitView
    {
        public ThreeWayUpperSplit()
        {
            InitializeComponent();
        }

        #region SplitView Members

        public Control FrontView
        {
            get
            {
                return horizontalSplit.Panel2;
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
                return verticalSplit.Panel1;
            }
        }

        public Control RightView
        {
            get
            {
                return verticalSplit.Panel2;
            }
        }

        #endregion
    }
}
