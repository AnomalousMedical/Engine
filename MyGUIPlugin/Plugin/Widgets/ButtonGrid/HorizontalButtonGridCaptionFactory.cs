using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class HorizontalButtonGridCaptionFactory : ButtonGridCaptionFactory
    {
        private ButtonGrid grid;

        public HorizontalButtonGridCaptionFactory(ButtonGrid grid)
        {
            this.grid = grid;
        }

        public ButtonGridCaption createCaption(String name)
        {
            TextBox captionText = grid.ScrollView.createWidgetT("TextBox", grid.GroupCaptionSkin, 0, 0, 10, 10, Align.Left | Align.Top, "") as TextBox;
            captionText.Font = grid.GroupCaptionFont;
            captionText.Caption = name;
            captionText.setSize((int)captionText.getTextSize().Width + 5, (int)captionText.FontHeight);
            captionText.ForwardMouseWheelToParent = true;

            Widget separator = grid.ScrollView.createWidgetT("Widget", grid.GroupSeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
            separator.setSize((int)(grid.ScrollView.CanvasSize.Width - captionText.Width) - 10, 1);
            separator.ForwardMouseWheelToParent = true;

            return new HorizontalButtonGridCaption(captionText, separator);
        }

        public void destroyCaption(ButtonGridCaption caption)
        {
            caption.Dispose();
        }
    }
}
