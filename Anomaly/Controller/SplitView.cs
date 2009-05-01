using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    interface SplitView : IDisposable
    {
        Control UpperLeft
        {
            get;
        }

        Control UpperRight
        {
            get;
        }

        Control LowerLeft
        {
            get;
        }

        Control LowerRight
        {
            get;
        }
    }
}
