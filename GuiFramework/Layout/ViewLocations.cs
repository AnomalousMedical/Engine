using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    [Flags]
    public enum ViewLocations
    {
        Left = 1,
        Right = 1 << 1,
        Top = 1 << 2,
        Bottom = 1 << 3,
        Floating = 1 << 4,
    }
}
