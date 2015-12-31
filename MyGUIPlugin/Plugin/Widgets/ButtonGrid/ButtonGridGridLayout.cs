using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ButtonGridGridLayout : ButtonGridLayout
    {
        IntVector2 currentPosition;
        IntSize2 canvasSize;
        ButtonGrid buttonGrid;
        bool allowNewlines = false;
        int calculatedPadding;
        int xStartPosition;
        bool center = true;

        public ButtonGridGridLayout()
        {
            
        }

        public void startLayout(ButtonGrid buttonGrid)
        {
            this.buttonGrid = buttonGrid;
            this.canvasSize = buttonGrid.ScrollView.CanvasSize;
            allowNewlines = false;

            int totalWidth = ItemWidth + ItemPaddingX;
            if(!center)
            {
                totalWidth -= ItemPaddingX; //Don't need padding on last element if not centered.
            }
            int numElements = canvasSize.Width / totalWidth;
            if (numElements > 0)
            {
                int totalElementWidth = numElements * totalWidth;
                calculatedPadding = (canvasSize.Width - totalElementWidth) / numElements;
                xStartPosition = center ? calculatedPadding / 2 : 0;
            }
            else
            {
                calculatedPadding = ItemPaddingX;
                xStartPosition = 0;
            }

            currentPosition = new IntVector2(xStartPosition, 0);
        }

        public void alignCaption(ButtonGridCaption caption)
        {
            caption.align(0, currentPosition.y, canvasSize.Width);
            currentPosition.y += caption.Height;
        }

        public void alignItem(ButtonGridItem item)
        {
            if (allowNewlines && currentPosition.x + ItemWidth > canvasSize.Width)
            {
                currentPosition.x = xStartPosition;
                currentPosition.y += ItemHeight + ItemPaddingY;
            }
            item.setPosition(currentPosition, ItemWidth, ItemHeight);
            currentPosition.x += item.Width + calculatedPadding;
            allowNewlines = true;
        }

        public void finishGroupLayout()
        {
            currentPosition.x = xStartPosition;
            currentPosition.y += ItemHeight + GroupPaddingY;
            allowNewlines = false;
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

        public bool Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }
    }
}
