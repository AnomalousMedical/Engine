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

public:
	ReshapeableRigidBody(btRigidBody* rigidBody, btCompoundShape* compoundShape);

	~ReshapeableRigidBody(void);

	/// <summary>
	/// Create a new hull region by decomposing the mesh in desc. If the region
	///  does not exist it will be created. If it does exist it will be cleared
	///  and recreated.
	/// </summary>
	/// <param name="name">The name of the region.</param>
	/// <param name="desc">The mesh description and algorithm configuration settings.</param>
	/// <param name="origin">An origin for the hull region.</param>
	/// <param name="orientation">An orientation for the hull region.</param>
	void createHullRegion(std::string name, ConvexDecompositionDesc* desc, Vector3* origin, Quaternion* orientation);

	/// <summary>
	/// Add a Sphere to the given region. If the region does not exist it will
	/// be created.
	/// </summary>
	/// <param name="sectionName">The name of the section to add the sphere to.</param>
	/// <param name="radius">The radius of the sphere.</param>
	/// <param name="origin">The origin of the sphere.</param>
	void addSphereShape(std::string regionName, float radius, Vector3* origin);

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