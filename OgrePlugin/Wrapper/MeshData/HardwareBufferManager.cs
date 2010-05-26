using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
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
