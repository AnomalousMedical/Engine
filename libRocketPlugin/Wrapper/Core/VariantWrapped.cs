using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    /// <summary>
    /// This is a variation of the variant that has been nativly allocated, so
    /// the dispose does nothing.
    /// </summary>
    class VariantWrapped : Variant
    {
        public VariantWrapped()
        {

        }

        public VariantWrapped(IntPtr ptr)
            :base(ptr)
        {

        }

        public override void Dispose()
        {
            //Does nothing, don't want to delete the nativly allocated variants
        }

        internal void changePointer(IntPtr ptr)
        {
            setPtr(ptr);
        }
    }
}
