﻿using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public sealed class BottomSeparatorButtonGridCaptionFactory : ButtonGridCaptionFactory
    {
        public BottomSeparatorButtonGridCaptionFactory()
        {
            CaptionSkin = "CaptionTextBox";
            SeparatorSkin = "SeparatorSkin";
            SeparatorTextSpace = ScaleHelper.Scaled(10);
            SeparatorAdditionalPaddingTop = ScaleHelper.Scaled(5);
            SeparatorAdditionalPaddingBottom = 0;
        }

        protected internal override ButtonGridCaption createCaption(String name)
        {
            TextBox captionText = Grid.ScrollView.createWidgetT("TextBox", CaptionSkin, 0, 0, 10, 10, Align.Left | Align.Top, "") as TextBox;
            captionText.Caption = name;
            captionText.setSize(captionText.getTextSize().Width, captionText.FontHeight);
            captionText.ForwardMouseWheelToParent = true;

            Widget separator = Grid.ScrollView.createWidgetT("Widget", SeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
            separator.setSize(Grid.ScrollView.CanvasSize.Width, 1);
            separator.ForwardMouseWheelToParent = true;

            return new BottomSeparatorButtonGridCaption(captionText, separator, this);
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

            int readInt;
            String read = scrollView.getUserString("GroupSeparatorTextSpace");
            if(!String.IsNullOrWhiteSpace(read) && NumberParser.TryParse(read, out readInt))
            {
                SeparatorTextSpace = ScaleHelper.Scaled(readInt);
            }

            read = scrollView.getUserString("GroupSeparatorAdditionalPaddingTop");
            if (!String.IsNullOrWhiteSpace(read) && NumberParser.TryParse(read, out readInt))
            {
                SeparatorAdditionalPaddingTop = ScaleHelper.Scaled(readInt);
            }

            read = scrollView.getUserString("GroupSeparatorAdditionalPaddingBottom");
            if (!String.IsNullOrWhiteSpace(read) && NumberParser.TryParse(read, out readInt))
            {
                SeparatorAdditionalPaddingBottom = ScaleHelper.Scaled(readInt);
            }
        }

        /// <summary>
        /// The skin for the group caption. Can be specified in layout as GroupCaptionSkin.
        /// </summary>
        public String CaptionSkin { get; set; }

        /// <summary>
        /// The skin for the group separators. Can be specified in layout as GroupSeparatorSkin.
        /// </summary>
        public String SeparatorSkin { get; set; }

        /// <summary>
        /// The amount of space between the separator and the group caption. Can be specified in the layout as GroupSeparatorTextSpace.
        /// </summary>
        public int SeparatorTextSpace { get; set; }

        /// <summary>
        /// The amount of space between the separator and the group caption. Can be specified in the layout as GroupSeparatorAdditionalPaddingTop.
        /// </summary>
        public int SeparatorAdditionalPaddingTop { get; set; }

        /// <summary>
        /// The amount of space between the separator and the group caption. Can be specified in the layout as GroupSeparatorAdditionalPaddingBottom.
        /// </summary>
        public int SeparatorAdditionalPaddingBottom { get; set; }
    }
}
