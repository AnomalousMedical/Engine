using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public class SharedPtr<T> : IDisposable
        where T : IDisposable
    {
        private T value;
        private SharedPtrCollection<T> owner;
        private IntPtr nativeObject;
        private IntPtr heapSharedPtr;
        private StackTrace st;

        internal SharedPtr(T value, IntPtr nativeObject, IntPtr heapSharedPtr, SharedPtrCollection<T> owner)
        {
            this.value = value;
            this.nativeObject = nativeObject;
            this.owner = owner;
            this.heapSharedPtr = heapSharedPtr;

            if (OgreInterface.TrackMemoryLeaks)
            {
                st = new StackTrace(true);
            }
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

        internal IntPtr HeapSharedPtr
        {
            get
            {
                return heapSharedPtr;
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
