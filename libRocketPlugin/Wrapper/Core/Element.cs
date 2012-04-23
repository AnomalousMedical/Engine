using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class Element : RocketNativeObject
    {
        public Element(IntPtr ptr)
            : base(ptr)
        {

        }

        internal static Element Wrap(IntPtr element)
        {
            if (element != IntPtr.Zero)
            {
                return new Element(element);
            }
            return null;
        }
    }
}
