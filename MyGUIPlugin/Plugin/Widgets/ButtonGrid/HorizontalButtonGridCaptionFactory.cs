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
            GroupCaptionSkin = "TextBox";
            GroupSeparatorSkin = "SeparatorSkin";
        }

        protected internal override ButtonGridCaption createCaption(String name)
        {
            TextBox captionText = Grid.ScrollView.createWidgetT("TextBox", GroupCaptionSkin, 0, 0, 10, 10, Align.Left | Align.Top, "") as TextBox;
            captionText.Caption = name;
            captionText.setSize((int)captionText.getTextSize().Width + TextSeparatorBorderSize, (int)captionText.FontHeight);
            captionText.ForwardMouseWheelToParent = true;

            Widget separator = Grid.ScrollView.createWidgetT("Widget", GroupSeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
            separator.setSize((int)(Grid.ScrollView.CanvasSize.Width - captionText.Width) - BorderWidthOffset, 1);
            separator.ForwardMouseWheelToParent = true;

            return new HorizontalButtonGridCaption(captionText, separator);
        }

        protected internal override void destroyCaption(ButtonGridCaption caption)
        {
            caption.Dispose();
        }

        protected override void gridSet()
        {
            ScrollView scrollView = Grid.ScrollView;

            GroupCaptionSkin = scrollView.getUserString("GroupCaptionSkin", GroupCaptionSkin);
            GroupSeparatorSkin = scrollView.getUserString("GroupSeparatorSkin", GroupSeparatorSkin);
        }

        /// <summary>
        /// The skin for the group caption.
        /// </summary>
        public String GroupCaptionSkin { get; set; }

        /// <summary>
        /// The skin for the group separators.
        /// </summary>
        public String GroupSeparatorSkin { get; set; }
    }
}
