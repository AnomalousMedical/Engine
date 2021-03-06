﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Anomalous.Interop;

namespace libRocketPlugin
{
    public class RktDictionary : RocketNativeObject, IEnumerable<RktEntry>
    {
        private VariantWrapped pooledVariant = new VariantWrapped();
        private RktDictionaryIterator iterator;

        internal RktDictionary()
        {
            iterator = new RktDictionaryIterator(this);
        }

        internal RktDictionary(IntPtr ptr)
        {
            iterator = new RktDictionaryIterator(this);
        }

        public bool Remove(String key)
        {
            return Dictionary_Remove(ptr, key);
        }

        public bool Reserve(int size)
        {
            return Dictionary_Reserve(ptr, ref size);
        }

        public void Clear()
        {
            Dictionary_Clear(ptr);
        }

        public void Merge(RktDictionary dict)
        {
            Dictionary_Merge(ptr, dict.Ptr);
        }

        public bool HasValue(String key)
        {
            return Dictionary_Get(ptr, key) != IntPtr.Zero;
        }

        public Variant this[String key]
        {
            get
            {
                pooledVariant.changePointer(Dictionary_Get(ptr, key));
                return pooledVariant;
            }
            set
            {
                Dictionary_Set(ptr, key, value.Ptr);
            }
        }

        public bool Empty
        {
            get
            {
                return Dictionary_IsEmpty(ptr);
            }
        }

        public int Size
        {
            get
            {
                return Dictionary_Size(ptr);
            }
        }

        internal void changePointer(IntPtr ptr)
        {
            setPtr(ptr);
        }

        internal bool Iterate(ref int pos, out String key, out Variant value)
        {
            using (StringRetriever stringRetriever = new StringRetriever())
            {
                IntPtr variantPtr = IntPtr.Zero;
                bool retVal = Dictionary_Iterate(ptr, ref pos, stringRetriever.StringCallback, ref variantPtr, stringRetriever.Handle);
                key = stringRetriever.retrieveString();
                pooledVariant.changePointer(variantPtr);
                value = pooledVariant;
                return retVal;
            }
        }

        public IEnumerator<RktEntry> GetEnumerator()
        {
            iterator.Reset();
            return iterator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Dictionary_Set(IntPtr dictionary, String key, IntPtr value);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Dictionary_Get(IntPtr dictionary, String key);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Dictionary_Remove(IntPtr dictionary, String key);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Dictionary_Iterate(IntPtr dictionary, ref int pos, StringRetriever.Callback keyCb, ref IntPtr value, IntPtr handle);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Dictionary_Reserve(IntPtr dictionary, ref int size);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Dictionary_Clear(IntPtr dictionary);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Dictionary_IsEmpty(IntPtr dictionary);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Dictionary_Size(IntPtr dictionary);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Dictionary_Merge(IntPtr dictionary, IntPtr dict);

        #endregion
    }
}
