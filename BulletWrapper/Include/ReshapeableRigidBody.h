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

	void cloneAndSetShape(std::string regionName, btCollisionShape* toClone, const Vector3& translation, const Quaternion& rotation, const Vector3& scale);

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