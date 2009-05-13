using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    class SplitView : UserControl
    {
        public virtual Control FrontView
        {
            get
            {
                return null;
            }
        }

        public virtual Control BackView
        {
            get
            {
                return null;
            }
        }

        public virtual Control LeftView
        {
            get
            {
                return null;
            }
        }

        public virtual Control RightView
        {
            get
            {
                return null;
            }
        }
    }
}
