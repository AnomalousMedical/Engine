using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public class SharpText : ILayoutItem
    {
        public string Text;
        public IntRect Rect;
        public Color color;
        public int Width = int.MaxValue;

        public SharpText()
        {

        }

        public SharpText(String text)
        {
            this.Text = text;
        }

        public void SetRect(String text, int left, int top, int width, int height)
        {
            this.Text = text;
            this.Rect.Left = left;
            this.Rect.Top = top;
            this.Rect.Width = width;
            this.Rect.Height = height;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            return sharpGui.MeasureText(Text);
        }

        public void SetRect(IntRect rect)
        {
            this.Rect = rect;
        }

        public SharpText UpdateText(String text)
        {
            this.Text = text;
            return this;
        }
    }
}
