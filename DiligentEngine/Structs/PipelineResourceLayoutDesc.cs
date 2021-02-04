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
    public partial class PipelineResourceLayoutDesc
    {

        public PipelineResourceLayoutDesc()
        {
            
        }
        public SHADER_RESOURCE_VARIABLE_TYPE DefaultVariableType { get; set; } = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC;
        public Uint32 NumVariables { get; set; } = 0;
        public Uint32 NumImmutableSamplers { get; set; } = 0;


    }
}
