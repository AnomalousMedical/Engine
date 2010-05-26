using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class MaterialManager : IDisposable
    {
        static MaterialManager instance = new MaterialManager();

        public static MaterialManager getInstance()
        {
            return instance;
        }

        public void Dispose()
        {

        }
    }
}

