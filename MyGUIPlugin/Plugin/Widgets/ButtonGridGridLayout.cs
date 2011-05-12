using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    class ButtonGridGridLayout : ButtonGridLayout
    {
        Vector2 currentPosition;
        Size2 canvasSize;

        public void startLayout(Size2 canvasSize)
        {
            currentPosition = new Vector2(0.0f, 0.0f);
            this.canvasSize = canvasSize;
        }

        public void alignCaption(Widget captionText, Widget separator)
        {
            currentPosition.y += 5;
            captionText.setPosition((int)currentPosition.x, (int)currentPosition.y);
            separator.setPosition((int)(currentPosition.x + captionText.Width), (int)(currentPosition.y + captionText.Height / 2));
            separator.setSize((int)(canvasSize.Width - captionText.Width) - 10, 1);
            currentPosition.y += captionText.Height + 5;
        }

        public void alignItem(ButtonGridItem item)
        {
            if (currentPosition.x + ItemWidth > canvasSize.Width)
            {
                currentPosition.x = 0;
                currentPosition.y += ItemHeight + ItemPaddingY;
            }
            item.setPosition(currentPosition, ItemWidth, ItemHeight);
            currentPosition.x += item.Width + ItemPaddingX;
        }

        public void finishGroupLayout()
        {
            currentPosition.x = 0;
            currentPosition.y += ItemHeight + ItemPaddingY;
        }

        public int FinalHeight
        {
            get
            {
                return (int)currentPosition.y;
            }
        }

        public int ItemWidth { get; set; }

        public int ItemHeight { get; set; }

        public int ItemPaddingX { get; set; }

        public int ItemPaddingY { get; set; }
    }
}
