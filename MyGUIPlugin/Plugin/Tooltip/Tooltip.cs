using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    class Tooltip : IDisposable
    {
        private StaticText tooltipWidget;

        public Tooltip(String text, int x, int y)
        {
            tooltipWidget = Gui.Instance.createWidgetT("StaticText", "Tooltip", x, y, 1, 1, Align.Default, "Overlapped", "") as StaticText;
            tooltipWidget.Caption = text;
            Size2 textSize = tooltipWidget.getTextSize();
            tooltipWidget.setSize((int)textSize.Width + 6, (int)textSize.Height + 4);
            tooltipWidget.Visible = false;
            int viewHeight = Gui.Instance.getViewHeight();
            if (tooltipWidget.Bottom > viewHeight)
            {
                viewHeight -= tooltipWidget.Bottom;
                tooltipWidget.setPosition(tooltipWidget.Left, tooltipWidget.Top + viewHeight);
            }
            int viewWidth = Gui.Instance.getViewWidth();
            if (tooltipWidget.Right > viewWidth)
            {
                viewWidth -= tooltipWidget.Right;
                tooltipWidget.setPosition(tooltipWidget.Left + viewWidth, tooltipWidget.Top);
            }
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(tooltipWidget);
            tooltipWidget = null;
        }

        public bool Visible
        {
            get
            {
                return tooltipWidget.Visible;
            }
            set
            {
                tooltipWidget.Visible = value;
            }
        }
    }
}
