﻿using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public sealed class HorizontalButtonGridCaptionFactory : ButtonGridCaptionFactory
    {
        private static readonly int TextSeparatorBorderSize = ScaleHelper.Scaled(5);
        public static readonly int BorderWidthOffset = TextSeparatorBorderSize * 2;

        public HorizontalButtonGridCaptionFactory()
        {
            CaptionSkin = "TextBox";
            SeparatorSkin = "SeparatorSkin";
        }

        protected internal override ButtonGridCaption createCaption(String name)
        {
            TextBox captionText = Grid.ScrollView.createWidgetT("TextBox", CaptionSkin, 0, 0, 10, 10, Align.Left | Align.Top, "") as TextBox;
            captionText.Caption = name;
            captionText.setSize(captionText.getTextSize().Width + TextSeparatorBorderSize, captionText.FontHeight);
            captionText.ForwardMouseWheelToParent = true;

            Widget separator = Grid.ScrollView.createWidgetT("Widget", SeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
            separator.setSize(Grid.ScrollView.CanvasSize.Width - captionText.Width - BorderWidthOffset, 1);
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

            CaptionSkin = scrollView.getUserString("GroupCaptionSkin", CaptionSkin);
            SeparatorSkin = scrollView.getUserString("GroupSeparatorSkin", SeparatorSkin);
        }

        /// <summary>
        /// The skin for the group caption. Can be specified in layout as GroupCaptionSkin.
        /// </summary>
        public String CaptionSkin { get; set; }

        /// <summary>
        /// The skin for the group separators. Can be specified in layout as GroupSeparatorSkin.
        /// </summary>
        public String SeparatorSkin { get; set; }
    }
}
