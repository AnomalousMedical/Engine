using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ButtonGridListLayout : ButtonGridLayout
    {
        Vector2 currentPosition;
        Size2 canvasSize;

        public void startLayout(ButtonGrid buttonGrid)
        {
            currentPosition = new Vector2(0.0f, 0.0f);
            int totalSize = buttonGrid.Count * (ItemHeight + ItemPaddingY);
            if (buttonGrid.ShowGroupCaptions)
            {
                totalSize += buttonGrid.CaptionHeight * buttonGrid.GroupCount;
            }

            //Set the height of the canvas, this allows us to get the width of the client region adjusted for scrollbars
            canvasSize = buttonGrid.ScrollView.CanvasSize;
            canvasSize.Height = totalSize;
            buttonGrid.ScrollView.CanvasSize = canvasSize;
            
            //Get the item width from the clientcoord
            ItemWidth = buttonGrid.ScrollView.ClientCoord.width;
            
            canvasSize.Width = ItemWidth;
        }

        public void alignCaption(Widget captionText, Widget separator)
        {
            currentPosition.y += 5;
            captionText.setPosition((int)currentPosition.x, (int)currentPosition.y);
            separator.setPosition((int)(currentPosition.x + captionText.Width), (int)(currentPosition.y + captionText.Height / 2));
            separator.setSize((int)(ItemWidth - captionText.Width) - 10, 1);
            currentPosition.y += captionText.Height + 5;
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
            currentPosition.y += ItemHeight + GroupPaddingY;
        }

        public Size2 FinalCanvasSize
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
