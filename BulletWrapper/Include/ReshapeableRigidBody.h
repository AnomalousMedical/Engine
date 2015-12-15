#pragma once

#include <string>
#include <map>

class ConvexDecompositionDesc;
class ReshapeableRigidBodySection;

typedef std::map<std::string, ReshapeableRigidBodySection*> HullRegionMap;

class ReshapeableRigidBody
{
private:
	btCompoundShape* compoundShape;
	btRigidBody* rigidBody;
	HullRegionMap hullRegions;
	ReshapeableRigidBodySection* getSection(std::string& name);

public:
	ReshapeableRigidBody(btRigidBody* rigidBody, btCompoundShape* compoundShape);

	~ReshapeableRigidBody(void);

	void cloneAndAddShape(std::string regionName, btCollisionShape* toClone, const Vector3& translation, const Quaternion& rotation, const Vector3& scale);

	void moveOrigin(std::string regionName, const Vector3& translation, const Quaternion& orientation);

	void setLocalScaling(std::string regionName, const Vector3& scale);

	/// <summary>
	/// Empty and destroy a region removing it from the collision shape.
	/// </summary>
	/// <param name="name">The name of the region to destroy.</param>
	void destroyRegion(std::string name);

	/// <summary>
	/// This function will recompute the mass props. It should be called when
	/// the collision shape is changed.
	/// </summary>
	void recomputeMassProps();
};


/// <summary>
/// The description for a convex decomposition.
/// </summary>
class ConvexDecompositionDesc
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