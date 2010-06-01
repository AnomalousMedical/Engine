using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    /// <summary>
    /// Vertex element semantics, used to identify the meaning of vertex buffer contents
    /// </summary>
    [SingleEnum]
    public enum VertexElementSemantic
    {
	    /// <summary>
	    /// Position, 3 reals per vertex
	    /// </summary>
	    VES_POSITION = 1,

	    /// <summary>
	    /// Blending weights
	    /// </summary>
	    VES_BLEND_WEIGHTS = 2,

	    /// <summary>
        /// Blending indices
	    /// </summary>
        VES_BLEND_INDICES = 3,

	    /// <summary>
	    /// Normal, 3 reals per vertex
	    /// </summary>
	    VES_NORMAL = 4,

	    /// <summary>
	    /// Diffuse colours
	    /// </summary>
	    VES_DIFFUSE = 5,

	    /// <summary>
	    /// Specular colours
	    /// </summary>
	    VES_SPECULAR = 6,

	    /// <summary>
	    /// Texture coordinates
	    /// </summary>
	    VES_TEXTURE_COORDINATES = 7,

	    /// <summary>
        /// Binormal (Y axis if normal is Z)
	    /// </summary>
        VES_BINORMAL = 8,

	    /// <summary>
        /// Tangent (X axis if normal is Z)
	    /// </summary>
        VES_TANGENT = 9
    };

    /// <summary>
    /// Vertex element type, used to identify the base types of the vertex contents
    /// </summary>
    [SingleEnum]
    public enum VertexElementType
    {
        VET_FLOAT1 = 0,
        VET_FLOAT2 = 1,
        VET_FLOAT3 = 2,
        VET_FLOAT4 = 3,
        /// alias to more specific colour type - use the current rendersystem's colour packing
	    VET_COLOUR = 4,
	    VET_SHORT1 = 5,
	    VET_SHORT2 = 6,
	    VET_SHORT3 = 7,
	    VET_SHORT4 = 8,
        VET_UBYTE4 = 9,
        /// D3D style compact colour
        VET_COLOUR_ARGB = 10,
        /// GL style compact colour
        VET_COLOUR_ABGR = 11
    };

    class VertexElement
    {
    }
}
