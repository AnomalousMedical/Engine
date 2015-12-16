#include "StdAfx.h"
#include "../Include/ReshapeableRigidBody.h"
#include "ReshapeableRigidBodySection.h"
#include "ConvexBuilder.h"

ReshapeableRigidBody::ReshapeableRigidBody(btRigidBody* rigidBody, btCompoundShape* compoundShape)
:compoundShape(compoundShape),
rigidBody(rigidBody)
{
}

ReshapeableRigidBody::~ReshapeableRigidBody(void)
{
	//Clear and delete all hullregions.
	for(HullRegionMap::iterator iter = hullRegions.begin(); iter != hullRegions.end(); ++iter)
	{
		iter->second->removeShapes(compoundShape);
		delete iter->second;
	}
	hullRegions.clear();
	
	//Delete the compound shape.
	if(compoundShape != 0)
	{
		delete compoundShape;
		compoundShape = 0;
	}
}

ReshapeableRigidBodySection* ReshapeableRigidBody::getSection(std::string& regionName)
{
	ReshapeableRigidBodySection* section;
	HullRegionMap::iterator sectionFind = hullRegions.find(regionName);

	if (sectionFind != hullRegions.end())
	{
		section = sectionFind->second;
	}
	else
	{
		section = new ReshapeableRigidBodySection();
		hullRegions[regionName] = section;
	}

	return section;
}

void ReshapeableRigidBody::cloneAndSetShape(std::string regionName, btCollisionShape* toClone, const Vector3& translation, const Quaternion& rotation, const Vector3& scale)
{
	ReshapeableRigidBodySection* section = getSection(regionName);
	section->moveOrigin(translation, rotation);
	section->cloneAndSetShape(toClone, compoundShape);
	section->setLocalScaling(scale);
	section->addShapes(compoundShape);
}

void ReshapeableRigidBody::moveOrigin(std::string regionName, const Vector3& translation, const Quaternion& orientation)
{
	ReshapeableRigidBodySection* section = getSection(regionName);
	section->removeShapes(compoundShape);
	section->moveOrigin(translation, orientation);
	section->addShapes(compoundShape);
}

void ReshapeableRigidBody::setLocalScaling(std::string regionName, const Vector3& scale)
{
	ReshapeableRigidBodySection* section = getSection(regionName);
	section->setLocalScaling(scale);
}

void ReshapeableRigidBody::destroyRegion(std::string name)
{
	//Find the section.
	ReshapeableRigidBodySection* section;
	HullRegionMap::iterator sectionFind = hullRegions.find(name);

	if(sectionFind != hullRegions.end())
	{
		section = sectionFind->second;
		section->removeShapes(compoundShape);
		hullRegions.erase(name);
		delete section;
	}
}

void ReshapeableRigidBody::recomputeMassProps()
{
	btVector3 localInertia;

	float mass = rigidBody->getInvMass();
	if(mass > 0.0f)
	{
		mass = 1.0f / mass;
	}

	compoundShape->calculateLocalInertia(mass, localInertia);
	rigidBody->setMassProps(mass, localInertia);
}


//C Wrapper
extern "C" _AnomalousExport ReshapeableRigidBody* ReshapeableRigidBody_Create(btRigidBody* rigidBody, btCompoundShape* compoundShape)
{
	return new ReshapeableRigidBody(rigidBody, compoundShape);
}

extern "C" _AnomalousExport void ReshapeableRigidBody_Delete(ReshapeableRigidBody* body)
{
	delete body;
}

extern "C" _AnomalousExport void ReshapeableRigidBody_cloneAndAddShape(ReshapeableRigidBody* body, char* regionName, btCollisionShape* toClone, const Vector3& translation, const Quaternion& rotation, const Vector3& scale)
{
	body->cloneAndSetShape(regionName, toClone, translation, rotation, scale);
}

extern "C" _AnomalousExport void ReshapeableRigidBody_destroyRegion(ReshapeableRigidBody* body, char* name)
{
	body->destroyRegion(name);
}

extern "C" _AnomalousExport void ReshapeableRigidBody_recomputeMassProps(ReshapeableRigidBody* body)
{
	body->recomputeMassProps();
}

extern "C" _AnomalousExport void ReshapeableRigidBody_moveOrigin(ReshapeableRigidBody* body, char* regionName, const Vector3& translation, const Quaternion& orientation)
{
	body->moveOrigin(regionName, translation, orientation);
}

extern "C" _AnomalousExport void ReshapeableRigidBody_setLocalScaling(ReshapeableRigidBody* body, char* regionName, const Vector3& scale)
{
	body->setLocalScaling(regionName, scale);
}