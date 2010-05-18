using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletPlugin
{
    /// <summary>
    /// The description for a convex decomposition.
    /// </summary>
    public unsafe class ConvexDecompositionDesc
    {
	    public ConvexDecompositionDesc()
	    {
		    mVcount = 0;
		    mVertices = (float*)0;
		    mTcount   = 0;
            mIndices = (uint*)0;
		    mDepth    = 5;
		    mCpercent = 5;
		    mPpercent = 5;
		    mMaxVertices = 32;
		    mSkinWidth   = 0;
	    }

	    // describes the input triangle.
	    public uint  mVcount;   // the number of vertices in the source mesh.
	    public float* mVertices; // start of the vertex position array.  Assumes a stride of 3 floats.
	    public uint  mTcount;   // the number of triangles in the source mesh.
	    public uint* mIndices;  // the indexed triangle list array (zero index based)

	    // options
	    public uint  mDepth;    // depth to split, a maximum of 10, generally not over 7.
	    public float         mCpercent; // the concavity threshold percentage.  0=20 is reasonable.
	    public float         mPpercent; // the percentage volume conservation threshold to collapse hulls. 0-30 is reasonable.

	    // hull output limits.
	    public uint  mMaxVertices; // maximum number of vertices in the output hull. Recommended 32 or less.
	    public float         mSkinWidth;   // a skin width to apply to the output hulls.
    }
}
