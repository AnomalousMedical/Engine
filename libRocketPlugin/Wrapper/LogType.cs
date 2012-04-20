using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public enum LogType
    {
        LT_ALWAYS = 0,
        LT_ERROR,
        LT_ASSERT,
        LT_WARNING,
        LT_INFO,
        LT_DEBUG,
        LT_MAX
    };
}
