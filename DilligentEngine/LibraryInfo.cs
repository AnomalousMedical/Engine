using System;
using System.Collections.Generic;
using System.Text;

namespace DilligentEngine
{
    static class LibraryInfo
    {
#if STATIC_LINK
		public const String LibraryName = "__Internal";
#else
        public const String LibraryName = "DilligentEngineWrapper";
#endif
    }
}
