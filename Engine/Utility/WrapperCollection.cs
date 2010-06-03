using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract unsafe class WrapperCollection<T> : IDisposable
{
    private	Dictionary<IntPtr, T> ptrDictionary = new Dictionary<IntPtr,T>();

    public WrapperCollection()
    {
        
    }

    public virtual void Dispose()
    {
        clearObjects();
    }

    public void clearObjects()
    {
        foreach(T obj in ptrDictionary.Values)
	    {
            ((IDisposable)obj).Dispose();
	    }
	    ptrDictionary.Clear();
    }

    protected T getObjectVoid(void* nativeObject, params object[] args)
    {
        IntPtr key = new IntPtr(nativeObject);
        if (!ptrDictionary.ContainsKey(key))
        {
            ptrDictionary.Add(key, createWrapper(nativeObject, args));
        }
        return ptrDictionary[key];
    }

    protected bool getObjectVoidNoCreate(void* nativeObject, ref T obj)
    {
        IntPtr key = new IntPtr(nativeObject);
        if (ptrDictionary.ContainsKey(key))
        {
            obj = ptrDictionary[key];
            return true;
        }
        return false;
    }

    protected void destroyObjectVoid(void* nativeObject)
    {
        IntPtr key = new IntPtr(nativeObject);
        if (ptrDictionary.ContainsKey(key))
        {
            ((IDisposable)ptrDictionary[key]).Dispose();
            ptrDictionary.Remove(key);
        }
    }

    protected abstract T createWrapper(void* nativeObject, object[] args);
}