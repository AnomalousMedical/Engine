using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipAccess
{
    internal class ZipLibraryInfo
    {
#if STATIC_LINK
		public const String Name = "__Internal";
#else
        public const String Name = "Zip";
#endif
    }
}
