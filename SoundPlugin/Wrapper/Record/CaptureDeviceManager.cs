using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace SoundPlugin
{
    class CaptureDeviceManager : IDisposable
    {
        private WrapperCollection<CaptureDevice> wrappers;

        public CaptureDeviceManager()
        {
            wrappers = new WrapperCollection<CaptureDevice>(createWrapper, destroyWrapper);
        }

        public CaptureDevice get(IntPtr ptr, OpenALManager openAlManager)
        {
            if (ptr != IntPtr.Zero)
            {
                return wrappers.getObject(ptr, openAlManager);
            }
            return null;
        }

        public void Dispose()
        {
            wrappers.clearObjects();
        }

        public IntPtr deleteWrapper(IntPtr widget)
        {
            return wrappers.destroyObject(widget);
        }

        private CaptureDevice createWrapper(IntPtr ptr, object[] args)
        {
            return new CaptureDevice(ptr, (OpenALManager)args[0]);
        }

        private void destroyWrapper(CaptureDevice source)
        {
            source.delete();
        }
    }
}
