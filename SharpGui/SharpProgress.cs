using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    public abstract class SharpProgress
    {
        public SharpProgress()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public IntRect Rect;
    }

    public class SharpProgressHorizontal : SharpProgress
    {

    }

    //public class SharpSliderVertical : SharpSlider
    //{
    //}
}
