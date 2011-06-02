using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public abstract class TableElement : IDisposable
    {
        public abstract void Dispose();

        public virtual Table Table { get; internal set; }
    }
}
