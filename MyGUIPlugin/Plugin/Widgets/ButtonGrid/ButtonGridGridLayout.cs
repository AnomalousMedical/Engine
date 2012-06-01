using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    class ButtonGridGridLayout : ButtonGridLayout
    {
        IntVector2 currentPosition;
        Size2 canvasSize;
        ButtonGrid buttonGrid;

        public void startLayout(ButtonGrid buttonGrid)
        {
            this.buttonGrid = buttonGrid;
            currentPosition = new IntVector2(0, 0);
            this.canvasSize = buttonGrid.ScrollView.CanvasSize;
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
            currentPosition.y += ItemHeight + GroupPaddingY;
        }

        public IntSize2 FinalCanvasSize
        {
            get
            {
                return new IntSize2(buttonGrid.ScrollView.CanvasSize.Width, currentPosition.y);
            }
        }

        public int ItemWidth { get; set; }

        public int ItemHeight { get; set; }

        public int ItemPaddingX { get; set; }

        public int ItemPaddingY { get; set; }

        public int GroupPaddingY { get; set; }
    }
}
