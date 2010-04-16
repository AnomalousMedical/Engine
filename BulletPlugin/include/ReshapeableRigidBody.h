#pragma once

#include "RigidBody.h"

namespace BulletPlugin
{

class ReshapeableRigidBodySection;

typedef System::Collections::Generic::Dictionary<System::String^, IntPtr> HullRegionMap;

ref class ReshapeableRigidBodyDefinition;
ref class ConvexDecompositionDesc;

public ref class ReshapeableRigidBody : public RigidBody
{
private:
	btCompoundShape* compoundShape;
	HullRegionMap hullRegions;

public:
	ReshapeableRigidBody(ReshapeableRigidBodyDefinition^ description, BulletScene^ scene, Vector3 initialTrans, Quaternion initialRot);

	virtual ~ReshapeableRigidBody(void);

	virtual SimElementDefinition^ saveToDefinition() override;

	void createHullRegion(System::String^ name, ConvexDecompositionDesc^ desc);

	void recomputeMassProps();
};

public ref class ConvexDecompositionDesc
{
	public:
		ConvexDecompositionDesc(void)
		{
			mVcount = 0;
			mVertices = 0;
			mTcount   = 0;
			mIndices  = 0;
			mDepth    = 5;
			mCpercent = 5;
			mPpercent = 5;
			mMaxVertices = 32;
			mSkinWidth   = 0;
		}

		// describes the input triangle.
		unsigned int  mVcount;   // the number of vertices in the source mesh.
		const float  *mVertices; // start of the vertex position array.  Assumes a stride of 3 floats.
		unsigned int  mTcount;   // the number of triangles in the source mesh.
		unsigned int *mIndices;  // the indexed triangle list array (zero index based)

		// options
		unsigned int  mDepth;    // depth to split, a maximum of 10, generally not over 7.
		float         mCpercent; // the concavity threshold percentage.  0=20 is reasonable.
		float         mPpercent; // the percentage volume conservation threshold to collapse hulls. 0-30 is reasonable.

		// hull output limits.
		unsigned int  mMaxVertices; // maximum number of vertices in the output hull. Recommended 32 or less.
		float         mSkinWidth;   // a skin width to apply to the output hulls.
};

}