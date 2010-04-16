#include "StdAfx.h"
#include "..\include\ReshapeableRigidBody.h"
#include "ReshapeableRigidBodyDefinition.h"
#include "ConvexDecomposition.h"
#include "ConvexBuilder.h"
#include "ReshapeableRigidBodySection.h"

namespace BulletPlugin
{

ReshapeableRigidBody::ReshapeableRigidBody(ReshapeableRigidBodyDefinition^ description, BulletScene^ scene, Vector3 initialTrans, Quaternion initialRot)
:RigidBody(description, scene, initialTrans, initialRot),
compoundShape(static_cast<btCompoundShape*>(description->ConstructionInfo->m_collisionShape))
{
}

ReshapeableRigidBody::~ReshapeableRigidBody(void)
{
	//Clear and delete all hullregions.
	for each(IntPtr ptr in hullRegions.Values)
	{
		ReshapeableRigidBodySection* section = static_cast<ReshapeableRigidBodySection*>(ptr.ToPointer());
		section->removeShapes(compoundShape);
		delete section;
	}
	hullRegions.Clear();

	//Delete the compound shape.
	if(compoundShape != 0)
	{
		delete compoundShape;
		compoundShape = 0;
	}
}

SimElementDefinition^ ReshapeableRigidBody::saveToDefinition()
{
	ReshapeableRigidBodyDefinition^ definition = gcnew ReshapeableRigidBodyDefinition(Name);
	fillOutDefinition(definition);
	return definition;
}

void ReshapeableRigidBody::createHullRegion(System::String^ name, ConvexDecompositionDesc^ desc)
{
	//Find the section.
	ReshapeableRigidBodySection* section;
	IntPtr ptr;
	if(hullRegions.TryGetValue(name, ptr))
	{
		section = static_cast<ReshapeableRigidBodySection*>(ptr.ToPointer());
	}
	else
	{
		section = new ReshapeableRigidBodySection();
		hullRegions.Add(name, IntPtr(section));
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

#pragma unmanaged

void computeMassProps(btRigidBody* rigidBody, btCollisionShape* collisionShape)
{
	btVector3 localInertia;

	float mass = rigidBody->getInvMass();
	if(mass > 0.0f)
	{
		mass = 1.0f / mass;
	}

	collisionShape->calculateLocalInertia(mass, localInertia);
	rigidBody->setMassProps(mass, localInertia);
}

#pragma managed

void ReshapeableRigidBody::recomputeMassProps()
{
	computeMassProps(this->Body, compoundShape);
}

}