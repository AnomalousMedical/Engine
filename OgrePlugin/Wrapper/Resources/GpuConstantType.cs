using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public enum GpuConstantType
    {
        GCT_FLOAT1 = 1,
        GCT_FLOAT2 = 2,
        GCT_FLOAT3 = 3,
        GCT_FLOAT4 = 4,
        GCT_SAMPLER1D = 5,
        GCT_SAMPLER2D = 6,
        GCT_SAMPLER3D = 7,
        GCT_SAMPLERCUBE = 8,
        GCT_SAMPLERRECT = 9,
        GCT_SAMPLER1DSHADOW = 10,
        GCT_SAMPLER2DSHADOW = 11,
        GCT_SAMPLER2DARRAY = 12,
        GCT_MATRIX_2X2 = 13,
        GCT_MATRIX_2X3 = 14,
        GCT_MATRIX_2X4 = 15,
        GCT_MATRIX_3X2 = 16,
        GCT_MATRIX_3X3 = 17,
        GCT_MATRIX_3X4 = 18,
        GCT_MATRIX_4X2 = 19,
        GCT_MATRIX_4X3 = 20,
        GCT_MATRIX_4X4 = 21,
        GCT_INT1 = 22,
        GCT_INT2 = 23,
        GCT_INT3 = 24,
        GCT_INT4 = 25,
        GCT_SUBROUTINE = 26,
        GCT_DOUBLE1 = 27,
        GCT_DOUBLE2 = 28,
        GCT_DOUBLE3 = 29,
        GCT_DOUBLE4 = 30,
        GCT_MATRIX_DOUBLE_2X2 = 31,
        GCT_MATRIX_DOUBLE_2X3 = 32,
        GCT_MATRIX_DOUBLE_2X4 = 33,
        GCT_MATRIX_DOUBLE_3X2 = 34,
        GCT_MATRIX_DOUBLE_3X3 = 35,
        GCT_MATRIX_DOUBLE_3X4 = 36,
        GCT_MATRIX_DOUBLE_4X2 = 37,
        GCT_MATRIX_DOUBLE_4X3 = 38,
        GCT_MATRIX_DOUBLE_4X4 = 39,
        GCT_UINT1 = 40,
        GCT_UINT2 = 41,
        GCT_UINT3 = 42,
        GCT_UINT4 = 43,
        GCT_BOOL1 = 44,
        GCT_BOOL2 = 45,
        GCT_BOOL3 = 46,
        GCT_BOOL4 = 47,
        GCT_SAMPLER_WRAPPER1D = 48,
        GCT_SAMPLER_WRAPPER2D = 49,
        GCT_SAMPLER_WRAPPER3D = 50,
        GCT_SAMPLER_WRAPPERCUBE = 51,
        GCT_SAMPLER_STATE = 52, //only for hlsl 4.0
        GCT_UNKNOWN = 99
    };
}
