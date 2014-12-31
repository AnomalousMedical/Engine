using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    public abstract class SingleChildLayoutContainer : LayoutContainer
    {
        public abstract LayoutContainer Child { get; set; }
    }
}
