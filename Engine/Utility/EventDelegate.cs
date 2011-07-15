using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A Generic event that can be used in place of EventHandler when no args are needed.
    /// </summary>
    public delegate void EventDelegate();

    /// <summary>
    /// A Generic event that can be used in place of EventHandler when no args are needed.
    /// </summary>
    public delegate void EventDelegate<SourceType>(SourceType source);

    /// <summary>
    /// A Generic event that can be used in place of EventHandler when no args are needed.
    /// </summary>
    public delegate void EventDelegate<SourceType, ArgType>(SourceType source, ArgType arg);
}
