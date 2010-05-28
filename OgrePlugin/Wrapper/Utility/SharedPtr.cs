using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class SharedPtr<T> : IDisposable
        where T : IDisposable
    {
        private T value;
        private SharedPtrCollection<T> owner;
        private StackTrace st;
        private IntPtr nativeObject;

        internal SharedPtr(T value, IntPtr nativeObject, SharedPtrCollection<T> owner)
        {
            this.value = value;
            this.nativeObject = nativeObject;
            this.owner = owner;
            st = new StackTrace(true);
        }

        public void Dispose()
        {
            owner.returnPtr(this);
        }

        public T Value
        {
            get
            {
                return value;
            }
        }

        internal IntPtr NativeObject
        {
            get
            {
                return nativeObject;
            }
        }

        internal StackTrace ST
        {
            get
            {
                return st;
            }
        }
    }
}
