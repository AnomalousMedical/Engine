using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class Element : ReferenceCountable
    {
        public Element(IntPtr ptr)
            : base(ptr)
        {

        }

        internal static Element WrapElement(IntPtr element)
        {
            if (element != IntPtr.Zero)
            {
                return new Element(element);
            }
            return null;
        }
    }
}
