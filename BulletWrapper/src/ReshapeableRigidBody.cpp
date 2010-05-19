#include "StdAfx.h"
#include "..\Include\ReshapeableRigidBody.h"
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

void ReshapeableRigidBody::createHullRegion(std::string name, ConvexDecompositionDesc* desc, Vector3* origin, Quaternion* orientation)
{
	//Find the section.
	ReshapeableRigidBodySection* section;
	HullRegionMap::iterator sectionFind = hullRegions.find(name);

	if(sectionFind != hullRegions.end())
	{
		section = sectionFind->second;
		section->moveOrigin(*origin, *orientation);
	}
	else
	{
		section = new ReshapeableRigidBodySection(*origin, *orientation);
		hullRegions[name] = section;
	}

	section->removeShapes(compoundShape);
	section->deleteShapes();

	//Build the hulls
	ConvexDecomposition::DecompDesc btDesc;
	btDesc.mVcount       =desc->mVcount;
	btDesc.mVertices     = desc->mVertices;
	btDesc.mTcount       = desc->mTcount;
	btDesc.mIndices      = desc->mIndices;
	btDesc.mDepth        = desc->mDepth;
	btDesc.mCpercent     = desc->mCpercent;
	btDesc.mPpercent     = desc->mPpercent;
	btDesc.mMaxVertices  = desc->mMaxVertices;
	btDesc.mSkinWidth    = desc->mSkinWidth;
	btDesc.mCallback = section;

	ConvexBuilder cb(btDesc.mCallback);
	cb.process(btDesc);

	section->addShapes(compoundShape);
}

void ReshapeableRigidBody::addSphereShape(std::string regionName, float radius, Vector3* origin)
{
	//Find the section.
	ReshapeableRigidBodySection* section;
	HullRegionMap::iterator sectionFind = hullRegions.find(regionName);

	if(sectionFind != hullRegions.end())
	{
		section = sectionFind->second;
	}
	else
	{
		section = new ReshapeableRigidBodySection();
		hullRegions[regionName] = section;
	}

	section->addSphere(radius, *origin, compoundShape);
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
		section->deleteShapes();
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
extern "C" _declspec(dllexport) ReshapeableRigidBody* ReshapeableRigidBody_Create(btRigidBody* rigidBody, btCompoundShape* compoundShape)
{
	return new ReshapeableRigidBody(rigidBody, compoundShape);
}

extern "C" _declspec(dllexport) void ReshapeableRigidBody_Delete(ReshapeableRigidBody* body)
{
	delete body;
}

extern "C" _declspec(dllexport) void ReshapeableRigidBody_createHullRegion(ReshapeableRigidBody* body, char* name, ConvexDecompositionDesc* desc, Vector3* origin, Quaternion* orientation)
{
	body->createHullRegion(name, desc, origin, orientation);
}

extern "C" _declspec(dllexport) void ReshapeableRigidBody_addSphereShape(ReshapeableRigidBody* body, char* regionName, float radius, Vector3* origin)
{
	body->addSphereShape(regionName, radius, origin);
}

extern "C" _declspec(dllexport) void ReshapeableRigidBody_destroyRegion(ReshapeableRigidBody* body, char* name)
{
	body->destroyRegion(name);
}

extern "C" _declspec(dllexport) void ReshapeableRigidBody_recomputeMassProps(ReshapeableRigidBody* body)
{
	body->recomputeMassProps();
}