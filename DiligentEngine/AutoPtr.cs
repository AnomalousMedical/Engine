using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngine
{
    /// <summary>
    /// This simulates the AutoPtr from the Diligent Engine. If you create or are given one of these
    /// you must dispose it. Either use a using or make your own class Disposable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoPtr<T> : IDisposable
        where T : IObject
    {
        private readonly T obj;

        public T Obj => obj;

        public AutoPtr(T obj)
        {
            obj.AddRef();
            this.obj = obj;
        }

        internal AutoPtr(T obj, bool addRef)
        {
            if (addRef)
            {
                obj.AddRef();
            }
            this.obj = obj;
        }

        public void Dispose()
        {
            obj.Release();
        }

        //public static implicit operator T(AutoPtr<T> d) => d.obj;
    }
}
