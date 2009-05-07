using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    interface SplitView : IDisposable
    {
        Control FrontView
        {
            get;
        }

        Control BackView
        {
            get;
        }

        Control LeftView
        {
            get;
        }

        Control RightView
        {
            get;
        }
    }
}
