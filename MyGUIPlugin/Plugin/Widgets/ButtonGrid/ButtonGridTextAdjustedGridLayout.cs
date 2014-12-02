using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ButtonGridTextAdjustedGridLayout : ButtonGridLayout
    {
        private IntVector2 currentPosition;
        private IntSize2 canvasSize;
        private ButtonGrid buttonGrid;
        private int extraPadding = 0;

        public ButtonGridTextAdjustedGridLayout()
        {

        }

        public ButtonGridTextAdjustedGridLayout(int extraPadding)
        {
            this.extraPadding = extraPadding;
        }

        public void startLayout(ButtonGrid buttonGrid)
        {
            this.buttonGrid = buttonGrid;
            currentPosition = new IntVector2(0, 0);
            this.canvasSize = buttonGrid.ScrollView.CanvasSize;
        }

        public void alignCaption(ButtonGridCaption caption)
        {
            currentPosition.y += 5;
            caption.align(currentPosition.x, currentPosition.y, (int)canvasSize.Width);
            currentPosition.y += caption.Height + 5;
        }

        public void alignItem(ButtonGridItem item)
        {
            int finalWidth = ItemWidth;
            int textSize = (int)item.TextSize.Width + 10 + extraPadding;
            if (textSize > finalWidth)
            {
                finalWidth = textSize;
            }
            if (currentPosition.x + finalWidth > canvasSize.Width)
            {
                currentPosition.x = 0;
                currentPosition.y += ItemHeight + ItemPaddingY;
            }
            item.setPosition(currentPosition, finalWidth, ItemHeight);
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
