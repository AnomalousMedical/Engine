using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    public abstract class SharpSlider
    {
        public SharpSlider()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public IntRect Rect;
        public int Max { get; set; }
    }

    public class SharpSliderHorizontal : SharpSlider
    {
        
    }

    public class SharpSliderVertical : SharpSlider
    {
    }
}
