#include "StdAfx.h"
#include "..\include\ReshapeableRigidBody.h"
#include "ReshapeableRigidBodyDefinition.h"
#include "ConvexDecomposition.h"
#include "ConvexBuilder.h"

namespace BulletPlugin
{

#pragma unmanaged

class MyConvexDecomposition : public ConvexDecomposition::ConvexDecompInterface
{
public:

	btAlignedObjectArray<btConvexHullShape*> m_convexShapes;
	btAlignedObjectArray<btVector3> m_convexCentroids;

	MyConvexDecomposition ()
	:mBaseCount(0),
	mHullCount(0)
	{
	}
	
	virtual void ConvexDecompResult(ConvexDecomposition::ConvexResult &result)
	{
		//btVector3 localScaling(6.f,6.f,6.f);
		btVector3 localScaling(1.f,1.f,1.f);

		//export data to .obj
		printf("ConvexResult. ");
		if (1)
		{
			printf("Saving");
			//fprintf(mOutputFile,"## Hull Piece %d with %d vertices and %d triangles.\r\n", mHullCount, result.mHullVcount, result.mHullTcount );

			//fprintf(mOutputFile,"usemtl Material%i\r\n",mBaseCount);
			//fprintf(mOutputFile,"o Object%i\r\n",mBaseCount);

			/*for (unsigned int i=0; i<result.mHullVcount; i++)
			{
				const float *p = &result.mHullVertices[i*3];
				fprintf(mOutputFile,"v %0.9f %0.9f %0.9f\r\n", p[0], p[1], p[2] );
			}*/

			//calc centroid, to shift vertices around center of mass
			btVector3 centroid(0,0,0);

			btAlignedObjectArray<btVector3> vertices;
			if ( 1 )
			{
				//const unsigned int *src = result.mHullIndices;
				for (unsigned int i=0; i<result.mHullVcount; i++)
				{
					btVector3 vertex(result.mHullVertices[i*3],result.mHullVertices[i*3+1],result.mHullVertices[i*3+2]);
					vertex *= localScaling;
					centroid += vertex;
				}
			}

			centroid *= 1.f/(float(result.mHullVcount) );

			if ( 1 )
			{
				//const unsigned int *src = result.mHullIndices;
				for (unsigned int i=0; i<result.mHullVcount; i++)
				{
					btVector3 vertex(result.mHullVertices[i*3],result.mHullVertices[i*3+1],result.mHullVertices[i*3+2]);
					vertex *= localScaling;
					vertex -= centroid ;
					vertices.push_back(vertex);
				}
			}
			
	

			if ( 1 )
			{
				const unsigned int *src = result.mHullIndices;
				for (unsigned int i=0; i<result.mHullTcount; i++)
				{
					unsigned int index0 = *src++;
					unsigned int index1 = *src++;
					unsigned int index2 = *src++;


					btVector3 vertex0(result.mHullVertices[index0*3], result.mHullVertices[index0*3+1],result.mHullVertices[index0*3+2]);
					btVector3 vertex1(result.mHullVertices[index1*3], result.mHullVertices[index1*3+1],result.mHullVertices[index1*3+2]);
					btVector3 vertex2(result.mHullVertices[index2*3], result.mHullVertices[index2*3+1],result.mHullVertices[index2*3+2]);
					vertex0 *= localScaling;
					vertex1 *= localScaling;
					vertex2 *= localScaling;
					
					vertex0 -= centroid;
					vertex1 -= centroid;
					vertex2 -= centroid;


					/*trimesh->addTriangle(vertex0,vertex1,vertex2);*/

					index0+=mBaseCount;
					index1+=mBaseCount;
					index2+=mBaseCount;
					
//				fprintf(mOutputFile,"f %d %d %d\r\n", index0+1, index1+1, index2+1 );
				}
			}

		//	float mass = 1.f;
			//float collisionMargin = 0.01f;

//this is a tools issue: due to collision margin, convex objects overlap, compensate for it here:
//#define SHRINK_OBJECT_INWARDS 1
#ifdef SHRINK_OBJECT_INWARDS

			
			std::vector<btVector3> planeEquations;
			btGeometryUtil::getPlaneEquationsFromVertices(vertices,planeEquations);

			std::vector<btVector3> shiftedPlaneEquations;
			for (int p=0;p<planeEquations.size();p++)
			{
				btVector3 plane = planeEquations[p];
				plane[3] += 5*collisionMargin;
				shiftedPlaneEquations.push_back(plane);
			}
			std::vector<btVector3> shiftedVertices;
			btGeometryUtil::getVerticesFromPlaneEquations(shiftedPlaneEquations,shiftedVertices);

			
			btConvexHullShape* convexShape = new btConvexHullShape(&(shiftedVertices[0].getX()),shiftedVertices.size());
			
#else //SHRINK_OBJECT_INWARDS
			
			btConvexHullShape* convexShape = new btConvexHullShape(&(vertices[0].getX()),vertices.size());
#endif 

			convexShape->setMargin(0.01f);
			m_convexShapes.push_back(convexShape);
			m_convexCentroids.push_back(centroid);
//			m_convexDemo->m_collisionShapes.push_back(convexShape);
			mBaseCount+=result.mHullVcount; // advance the 'base index' counter.


		}
	}

	int   	mBaseCount;
	int		mHullCount;

};

#pragma managed

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

#pragma unmanaged

void addShapes(btCompoundShape* compoundShape, MyConvexDecomposition* convexDecomposition)
{
	btTransform trans;
	trans.setIdentity();
	for (int i=0;i<convexDecomposition->m_convexShapes.size();i++)
	{
		
		btVector3 centroid = convexDecomposition->m_convexCentroids[i];
		trans.setOrigin(centroid);
		btConvexHullShape* convexShape = convexDecomposition->m_convexShapes[i];
		compoundShape->addChildShape(trans,convexShape);

	}
}

#pragma managed

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
	MyConvexDecomposition* convexDecomposition = new MyConvexDecomposition();
	btDesc.mCallback = convexDecomposition;

	ConvexBuilder cb(btDesc.mCallback);
	cb.process(btDesc);

	//Add the shapes OMG THE POINTER ISSUES THE WHOLE ALGORITHM NEEDS TO BE LOOKED AT
	addShapes(compoundShape, convexDecomposition);

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