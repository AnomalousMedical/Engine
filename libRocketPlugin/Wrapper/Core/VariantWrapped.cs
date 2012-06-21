using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    /// <summary>
    /// This is a variation of the variant that has been nativly allocated. It
    /// does not need to be disposed. It is also not publicly acessible.
    /// </summary>
    class VariantWrapped : Variant
    {
        /// <summary>
        /// Construct a variant or return null depending on what is needed.
        /// </summary>
        /// <param name="variant">The variant pointer to check.</param>
        /// <returns>A VariantWrapped or null.</returns>
        public static VariantWrapped Construct(IntPtr variant)
        {
            if (variant != IntPtr.Zero)
            {
                return new VariantWrapped(variant);
            }
            return null;
        }

        internal VariantWrapped()
            :base(IntPtr.Zero)
        {

        }

        private VariantWrapped(IntPtr ptr)
            :base(ptr)
        {

        }

        internal void changePointer(IntPtr ptr)
        {
            setPtr(ptr);
        }
    }
}
