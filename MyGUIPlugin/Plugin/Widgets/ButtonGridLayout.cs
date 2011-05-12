using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public interface ButtonGridLayout
    {
        void startLayout(ButtonGrid buttonGrid);

        void alignCaption(Widget text, Widget separator);

        void alignItem(ButtonGridItem item);

        void finishGroupLayout();

        Size2 FinalCanvasSize { get; }

        int ItemWidth { get; set; }

        int ItemHeight { get; set; }

        int ItemPaddingX { get; set; }

        int ItemPaddingY { get; set; }
    }
}
