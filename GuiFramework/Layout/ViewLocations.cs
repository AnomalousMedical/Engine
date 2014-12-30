using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical
{
    [SingleEnum]
    public enum ViewLocations
    {
        Left,
        Right,
        Top,
        Bottom,
        Floating,
    }
}
