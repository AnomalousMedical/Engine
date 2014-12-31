using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    public interface Notification
    {
        void clicked();

        String ImageKey { get; }

        String Text { get; }

        double Timeout { get; }
    }
}
