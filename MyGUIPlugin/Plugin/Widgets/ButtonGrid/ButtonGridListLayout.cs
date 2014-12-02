using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ButtonGridListLayout : ButtonGridLayout
    {
        IntVector2 currentPosition;
        IntSize2 canvasSize;

        public void startLayout(ButtonGrid buttonGrid)
        {
            currentPosition = new IntVector2(0, 0);
            int totalSize = buttonGrid.Count * (ItemHeight + ItemPaddingY);
            if (buttonGrid.ShowGroupCaptions)
            {
                totalSize += buttonGrid.CaptionHeight * buttonGrid.NonEmptyGroupCount;
            }

            //Set the height of the canvas, this allows us to get the width of the client region adjusted for scrollbars
            canvasSize = buttonGrid.ScrollView.CanvasSize;
            canvasSize.Height = totalSize;
            buttonGrid.ScrollView.CanvasSize = canvasSize;
            
            //Get the item width from the ViewCoord
            ItemWidth = buttonGrid.ScrollView.ViewCoord.width;
            
            canvasSize.Width = ItemWidth;
        }

        public void alignCaption(ButtonGridCaption caption)
        {
            caption.align(currentPosition.x, currentPosition.y, ItemWidth);
            currentPosition.y += caption.Height;
        }

        public void alignItem(ButtonGridItem item)
        {
            item.setPosition(currentPosition, ItemWidth, ItemHeight);
            currentPosition.x = 0;
            currentPosition.y += ItemHeight + ItemPaddingY;
        }

        public void finishGroupLayout()
        {
            currentPosition.x = 0;
        }

        public IntSize2 FinalCanvasSize
        {
            get
            {
                return canvasSize;
            }
        }

        public int ItemWidth { get; set; }

        public int ItemHeight { get; set; }

        public int ItemPaddingX { get; set; }

        public int ItemPaddingY { get; set; }

        public int GroupPaddingY { get; set; }
    }
}
