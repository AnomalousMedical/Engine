using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class RktEntry
    {
        internal String key;
        internal Variant value;

        public RktEntry()
        {

        }

        public String Key
        {
            get
            {
                return key;
            }
        }

        public Variant Value
        {
            get
            {
                return value;
            }
        }
    }
}
