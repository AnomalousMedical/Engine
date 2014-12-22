using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    internal static class UIntPtrExtensions
    {
        /// <summary>
        /// This function will convert the uintptr to a 32 bit uint in a way that
        /// is safe on 32 and 64 bit clrs. However, on 64 bit you could be outside of
        /// the valid range and get invalid values. This is unlikely to happen in practice,
        /// but is something to be aware of.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static uint horriblyUnsafeToUInt32(this UIntPtr ptr)
        {
            return (uint)ptr.ToUInt64();
        }
    }
}
