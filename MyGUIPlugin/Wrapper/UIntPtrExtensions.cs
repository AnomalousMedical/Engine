using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    internal static class UIntPtrExtensions
    {
        public static uint horriblyUnsafeToUInt32(this UIntPtr ptr)
        {
            return (uint)ptr.ToUInt64();
        }
    }
}
