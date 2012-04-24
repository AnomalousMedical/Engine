using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class Event : RocketNativeObject
    {
        public Event()
        {

        }

        public Event(IntPtr evt)
            :base(evt)
        {

        }

        public void changePtr(IntPtr evt)
        {
            setPtr(evt);
        }
    }
}
