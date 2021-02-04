using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;

namespace DiligentEngine
{
    public partial class DrawAttribs
    {

        public DrawAttribs()
        {
            
        }
        public Uint32 NumVertices { get; set; } = 0;
        public DRAW_FLAGS Flags { get; set; } = DRAW_FLAGS.DRAW_FLAG_NONE;
        public Uint32 NumInstances { get; set; } = 1;
        public Uint32 StartVertexLocation { get; set; } = 0;
        public Uint32 FirstInstanceLocation { get; set; } = 0;


    }
}
