using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    public abstract class SharpProgress : ILayoutItem
    {
        public SharpProgress()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public IntRect Rect;

        public IntSize2 DesiredSize;

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            return DesiredSize;
        }

        public void SetRect(IntRect rect)
        {
            this.Rect = rect;
        }
    }

    public class SharpProgressHorizontal : SharpProgress
    {

    }

    //public class SharpSliderVertical : SharpSlider
    //{
    //}
}
