using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    /// <summary>
    /// Implementation of a GLTF PBR renderer
    /// </summary>
    public partial class GLTF_PBR_Renderer
    {
        internal protected IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public GLTF_PBR_Renderer(IntPtr objPtr)
        {
            this.objPtr = objPtr;
        }


    }
}
