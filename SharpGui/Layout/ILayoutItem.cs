using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public interface ILayoutItem
    {
        IntSize2 GetDesiredSize(ISharpGui sharpGui);

        void SetRect(in IntRect rect);
    }
}
