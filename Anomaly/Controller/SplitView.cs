using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    interface SplitView
    {
        void initialize(SplitViewController controller);

        UserControl getControl();

        Control getSplitControl();
    }
}
