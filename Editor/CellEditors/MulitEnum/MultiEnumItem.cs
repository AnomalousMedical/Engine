using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    class MultiEnumItem
    {
        public MultiEnumItem(String text, ulong value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }

        public String Text { get; set; }

        public ulong Value { get; set; }
    }
}
