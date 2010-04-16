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

	//there are going to be major pointer problems here with the created hulls
	ReshapeableRigidBodySection* convexDecomposition = new ReshapeableRigidBodySection();
	btDesc.mCallback = convexDecomposition;

	ConvexBuilder cb(btDesc.mCallback);
	cb.process(btDesc);

	//Add the shapes OMG THE POINTER ISSUES THE WHOLE ALGORITHM NEEDS TO BE LOOKED AT
	convexDecomposition->addShapes(compoundShape);

	delete convexDecomposition;
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