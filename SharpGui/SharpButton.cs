using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    public class SharpButton : ILayoutItem
    {
        public SharpButton()
        {
            Id = Guid.NewGuid();
        }

        public void SetRect(int left, int top, int width, int height)
        {
            this.Rect.Left = left;
            this.Rect.Top = top;
            this.Rect.Width = width;
            this.Rect.Height = height;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            return sharpGui.MeasureButton(this);
        }

        public void SetRect(in IntRect rect)
        {
            this.Rect = rect;
        }

        public Guid Id { get; private set; }

        public IntRect Rect;
        
        public String Text { get; set; }
    }

    public class SharpButton<T> : SharpButton
    {
        public T UserObject { get; set; }
    }
}
