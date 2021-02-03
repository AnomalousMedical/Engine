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

namespace DiligentEngine
{
    public enum PRIMITIVE_TOPOLOGY :  Uint8
    {
        PRIMITIVE_TOPOLOGY_UNDEFINED = 0,
        PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
        PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP,
        PRIMITIVE_TOPOLOGY_POINT_LIST,
        PRIMITIVE_TOPOLOGY_LINE_LIST,
        PRIMITIVE_TOPOLOGY_LINE_STRIP,
        PRIMITIVE_TOPOLOGY_1_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_2_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_3_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_4_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_5_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_6_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_7_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_8_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_9_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_10_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_11_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_12_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_13_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_14_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_15_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_16_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_17_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_18_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_19_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_20_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_21_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_22_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_23_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_24_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_25_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_26_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_27_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_28_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_29_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_30_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_31_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_32_CONTROL_POINT_PATCHLIST,
        PRIMITIVE_TOPOLOGY_NUM_TOPOLOGIES,
    }
}