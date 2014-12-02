using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class HorizontalButtonGridCaptionFactory : ButtonGridCaptionFactory
    {
        private static readonly int TextSeparatorBorderSize = ScaleHelper.Scaled(5);
        public static readonly int BorderWidthOffset = TextSeparatorBorderSize * 2;

        public HorizontalButtonGridCaptionFactory()
        {
            
        }

        protected internal override ButtonGridCaption createCaption(String name)
        {
            TextBox captionText = Grid.ScrollView.createWidgetT("TextBox", Grid.GroupCaptionSkin, 0, 0, 10, 10, Align.Left | Align.Top, "") as TextBox;
            captionText.Font = Grid.GroupCaptionFont;
            captionText.Caption = name;
            captionText.setSize((int)captionText.getTextSize().Width + TextSeparatorBorderSize, (int)captionText.FontHeight);
            captionText.ForwardMouseWheelToParent = true;

            Widget separator = Grid.ScrollView.createWidgetT("Widget", Grid.GroupSeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
            separator.setSize((int)(Grid.ScrollView.CanvasSize.Width - captionText.Width) - BorderWidthOffset, 1);
            separator.ForwardMouseWheelToParent = true;

            return new HorizontalButtonGridCaption(captionText, separator);
        }

        protected internal override void destroyCaption(ButtonGridCaption caption)
        {
            caption.Dispose();
        }
    }
}
