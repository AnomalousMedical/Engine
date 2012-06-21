using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin.Wrapper.Core
{
    /// <summary>
    /// This variant is to be used by external libraries to pass data into the
    /// libRocket library that needs a variant. You MUST dispose the object
    /// yourself in order to do this correctly.
    /// </summary>
    class VariantAllocated : Variant, IDisposable
    {
        public VariantAllocated()
            :base(Variant_Create())
        {

        }

        public virtual void Dispose()
        {
            Variant_Delete(ptr);
        }
    }
}
