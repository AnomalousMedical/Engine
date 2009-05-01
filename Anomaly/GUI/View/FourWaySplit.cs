using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine;
using Engine.ObjectManagement;

namespace Anomaly
{
    partial class FourWaySplit : UserControl, SplitView
    {
        public FourWaySplit()
        {
            InitializeComponent();
        }

        #region SplitView Members

        public Control UpperLeft
        {
            get
            {
                return leftVertical.Panel1;
            }
        }

        public Control UpperRight
        {
            get
            {
                return rightVertical.Panel1;
            }
        }

        public Control LowerLeft
        {
            get
            {
                return leftVertical.Panel2;
            }
        }

        public Control LowerRight
        {
            get
            {
                return rightVertical.Panel2;
            }
        }

        #endregion
    }
}
