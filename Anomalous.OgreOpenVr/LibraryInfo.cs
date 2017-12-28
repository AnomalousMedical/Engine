using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OgreOpenVr
{
    static class LibraryInfo
    {
#if STATIC_LINK
		public const String Name = "__Internal";
#else
        public const String Name = "OgreOpenVr";
#endif
    }
}
