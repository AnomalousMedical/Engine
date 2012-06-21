using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace libRocketPlugin
{
    /// <summary>
    /// This is a wrapper for the variant class. There are two concrete
    /// versions. The first is VariantWrapped, which cannot be created outside
    /// this library and will appear as just a Variant when returned. These do
    /// not need to be disposed since they wrap native classes themselves. If
    /// you need to pass data into a function that takes a Variant use
    /// VariantAllocated, which will need to be disposed since you created an
    /// object on the unmanaged heap when you allocated that object.
    /// </summary>
    public abstract class Variant : RocketNativeObject
    {
        private static StringRetriever stringRetriever = new StringRetriever();

        /// Type of data stored in the variant.
        public enum Type
        {
            NONE = '-',
            BYTE = 'b',
            CHAR = 'c',
            FLOAT = 'f',
            INT = 'i',
            STRING = 's',
            WORD = 'w',
            VECTOR2 = '2',
            COLOURF = 'g',
            COLOURB = 'h',
            SCRIPTINTERFACE = 'p',
            VOIDPTR = '*',
        }

        protected Variant(IntPtr ptr)
            :base(ptr)
        {

        }

        public void Clear()
        {
            Variant_Clear(ptr);
        }

        public Type VariantType
        {
            get
            {
                return Variant_GetType(ptr);
            }
        }

        public byte ByteValue
        {
            get
            {
                return Variant_Get_Byte(ptr);
            }
            set
            {
                Variant_Set_Byte(ptr, value);
            }
        }

        public char CharValue
        {
            get
            {
                return Variant_Get_Char(ptr);
            }
            set
            {
                Variant_Set_Char(ptr, value);
            }
        }

        public float FloatValue
        {
            get
            {
                return Variant_Get_Float(ptr);
            }
            set
            {
                Variant_Set_Float(ptr, value);
            }
        }

        public int IntValue
        {
            get
            {
                return Variant_Get_Int(ptr);
            }
            set
            {
                Variant_Set_Int(ptr, value);
            }
        }

        public ushort WordValue
        {
            get
            {
                return Variant_Get_Word(ptr);
            }
            set
            {
                Variant_Set_Word(ptr, value);
            }
        }

        public String StringValue
        {
            get
            {
                Variant_Get_String(ptr, stringRetriever.StringCallback);
                return stringRetriever.retrieveString();
            }
            set
            {
                Variant_Set_String(ptr, value);
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        protected static extern IntPtr Variant_Create();

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Variant_Delete(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Clear(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern Type Variant_GetType(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Set_Byte(IntPtr variant, byte value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Set_Char(IntPtr variant, char value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Set_Float(IntPtr variant, float value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Set_Int(IntPtr variant, int value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Set_Word(IntPtr variant, ushort value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Set_String(IntPtr variant, String value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte Variant_Get_Byte(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern char Variant_Get_Char(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Variant_Get_Float(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Variant_Get_Int(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort Variant_Get_Word(IntPtr variant);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Variant_Get_String(IntPtr variant, StringRetriever.Callback stringCb);

        #endregion
    }
}
