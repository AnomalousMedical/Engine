using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin
{
    public class ImageSet : IDisposable
    {
        internal static ImageSet createWrapper(IntPtr nativeObject, object[] args)
        {
            return new ImageSet(nativeObject);
        }

        IntPtr imageSet;

        private ImageSet(IntPtr imageSet)
        {
            this.imageSet = imageSet;
        }

        public void Dispose()
        {
            imageSet = IntPtr.Zero;
        }

        internal IntPtr ImageSetPtr
        {
            get
            {
                return imageSet;
            }
        }
    }
}
