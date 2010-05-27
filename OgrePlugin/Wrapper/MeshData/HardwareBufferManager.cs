using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class HardwareBufferManager : IDisposable
    {
        static HardwareBufferManager instance = new HardwareBufferManager();

        public static HardwareBufferManager getInstance()
        {
            return instance;
        }

        public void Dispose()
        {

        }
    }
}
