using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public static class RTId
    {
        public static String CreateId(String category)
        {
#if DEBUG
            return $"{category}{Guid.NewGuid().ToString("N")}";
#else
            return Guid.NewGuid().ToString("N");
#endif
        }
    }
}
