#include "StdAfx.h"
#include "../Include/ReshapeableRigidBodySection.h"

ReshapeableRigidBodySection::ReshapeableRigidBodySection(void)
{
	transform.setIdentity();
}

ReshapeableRigidBodySection::ReshapeableRigidBodySection(const Vector3& translation, const Quaternion& orientation)
{
	transform.setOrigin(translation.toBullet());
	transform.setRotation(orientation.toBullet());
}

ReshapeableRigidBodySection::~ReshapeableRigidBodySection(void)
{
	deleteShapes();
}

void ReshapeableRigidBodySection::addShapes(btCompoundShape* compoundShape)
{
	btTransform trans;
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		trans.setIdentity();
		btVector3 centroid = m_convexCentroids[i];
		trans.setOrigin(centroid);
		trans = transform * trans;
		btCollisionShape* convexShape = m_convexShapes[i];
		compoundShape->addChildShape(trans, convexShape);
	}
}

void ReshapeableRigidBodySection::removeShapes(btCompoundShape* compoundShape)
{
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		btCollisionShape* convexShape = m_convexShapes[i];
		compoundShape->removeChildShape(convexShape);
	}
}

void ReshapeableRigidBodySection::deleteShapes()
{
	for (int i=0; i < m_convexShapes.size(); ++i)
	{
		btCollisionShape* convexShape = m_convexShapes[i];
		delete convexShape;
	}
	m_convexShapes.clear();
	m_convexCentroids.clear();
}

void ReshapeableRigidBodySection::addSphere(float radius, const Vector3& translation, btCompoundShape* compoundShape)
{
	btSphereShape* sphereShape = new btSphereShape(radius);
	btVector3 centroid = translation.toBullet();
	m_convexShapes.push_back(sphereShape);
	m_convexCentroids.push_back(centroid);
	if(compoundShape != 0)
	{
		btTransform transform;
		transform.setIdentity();
		transform.setOrigin(centroid);
		compoundShape->addChildShape(transform, sphereShape);
	}
}

void ReshapeableRigidBodySection::addHullShape(float* vertices, int numPoints, int stride, float collisionMargin, const Vector3& translation, const Quaternion& rotation, btCompoundShape* compoundShape)
{
	btConvexHullShape* shape = new btConvexHullShape(vertices, numPoints, stride);
	shape->setMargin(collisionMargin);

	btVector3 centroid = translation.toBullet();
	m_convexShapes.push_back(shape);
	m_convexCentroids.push_back(centroid);
	if (compoundShape != 0)
	{
		btTransform transform;
		transform.setIdentity();
		transform.setOrigin(centroid);
		transform.setRotation(rotation.toBullet());
		compoundShape->addChildShape(transform, shape);
	}
}

#include "../Extras/Serialize/BulletFileLoader/btBulletFile.h"

void ReshapeableRigidBodySection::cloneAndAddShape(btCollisionShape* toClone, const Vector3& translation, const Quaternion& rotation, btCompoundShape* compoundShape)
{
	btDefaultSerializer* serializer = new btDefaultSerializer();

	serializer->startSerialization();
	toClone->serializeSingleShape(serializer);
	serializer->finishSerialization();

	bParse::btBulletFile* bulletFile2 = new bParse::btBulletFile((char*)serializer->getBufferPointer(), serializer->getCurrentBufferSize());

	if (bulletFile2->m_collisionShapes.size() > 0)
	{
		btCollisionShapeData* shapeData = (btCollisionShapeData*)bulletFile2->m_collisionShapes[0];
		btCollisionShape* shape = convertCollisionShape(shapeData);

		btVector3 centroid = translation.toBullet();
		m_convexShapes.push_back(shape);
		m_convexCentroids.push_back(centroid);
		if (compoundShape != 0)
		{
			btTransform transform;
			transform.setIdentity();
			transform.setOrigin(centroid);
			transform.setRotation(rotation.toBullet());
			compoundShape->addChildShape(transform, shape);
		}
	}

	delete bulletFile2;

	delete serializer;
}

void ReshapeableRigidBodySection::moveOrigin(const Vector3& translation, const Quaternion& orientation)
{
	transform.setOrigin(translation.toBullet());
	transform.setRotation(orientation.toBullet());
}

void ReshapeableRigidBodySection::setLocalScaling(const Vector3& scale)
{
	btVector3 btScale = scale.toBullet();
	for (int i = 0; i < m_convexShapes.size(); ++i)
	{
		btCollisionShape* convexShape = m_convexShapes[i];
		convexShape->setLocalScaling(btScale);
	}
}

void ReshapeableRigidBodySection::ConvexDecompResult(ConvexDecomposition::ConvexResult &result)
{
	btVector3 localScaling(1.f,1.f,1.f);

	//calc centroid, to shift vertices around center of mass
	btVector3 centroid(0,0,0);
	btAlignedObjectArray<btVector3> vertices;
	for (unsigned int i=0; i<result.mHullVcount; i++)
	{
		btVector3 vertex(result.mHullVertices[i*3],result.mHullVertices[i*3+1],result.mHullVertices[i*3+2]);
		vertex *= localScaling;
		centroid += vertex;
	}

	centroid *= 1.f/(float(result.mHullVcount) );

	//Shift vertices by centroid.
	for (unsigned int i=0; i<result.mHullVcount; i++)
	{
		btVector3 vertex(result.mHullVertices[i*3],result.mHullVertices[i*3+1],result.mHullVertices[i*3+2]);
		vertex *= localScaling;
		vertex -= centroid ;
		vertices.push_back(vertex);
	}

	//Create convex hull
	btConvexHullShape* convexShape = new btConvexHullShape(&(vertices[0].getX()),vertices.size());

	convexShape->setMargin(0.0f);
	m_convexShapes.push_back(convexShape);
	m_convexCentroids.push_back(centroid);
}

btCollisionShape* ReshapeableRigidBodySection::convertCollisionShape(btCollisionShapeData* shapeData)
{
	btCollisionShape* shape = 0;

	switch (shapeData->m_shapeType)
	{
	case STATIC_PLANE_PROXYTYPE:
	{
		btStaticPlaneShapeData* planeData = (btStaticPlaneShapeData*)shapeData;
		btVector3 planeNormal, localScaling;
		planeNormal.deSerializeFloat(planeData->m_planeNormal);
		localScaling.deSerializeFloat(planeData->m_localScaling);
		shape = createPlaneShape(planeNormal, planeData->m_planeConstant);
		shape->setLocalScaling(localScaling);

		break;
	}
	case SCALED_TRIANGLE_MESH_SHAPE_PROXYTYPE:
	{
		btScaledTriangleMeshShapeData* scaledMesh = (btScaledTriangleMeshShapeData*)shapeData;
		btCollisionShapeData* colShapeData = (btCollisionShapeData*)&scaledMesh->m_trimeshShapeData;
		colShapeData->m_shapeType = TRIANGLE_MESH_SHAPE_PROXYTYPE;
		btCollisionShape* childShape = convertCollisionShape(colShapeData);
		btBvhTriangleMeshShape* meshShape = (btBvhTriangleMeshShape*)childShape;
		btVector3 localScaling;
		localScaling.deSerializeFloat(scaledMesh->m_localScaling);

		shape = createScaledTrangleMeshShape(meshShape, localScaling);
		break;
	}
	case GIMPACT_SHAPE_PROXYTYPE:
	{
		//btGImpactMeshShapeData* gimpactData = (btGImpactMeshShapeData*)shapeData;
		//if (gimpactData->m_gimpactSubType == CONST_GIMPACT_TRIMESH_SHAPE)
		//{
		//	btStridingMeshInterfaceData* interfaceData = createStridingMeshInterfaceData(&gimpactData->m_meshInterface);
		//	btTriangleIndexVertexArray* meshInterface = createMeshInterface(*interfaceData);


		//	btGImpactMeshShape* gimpactShape = createGimpactShape(meshInterface);
		//	btVector3 localScaling;
		//	localScaling.deSerializeFloat(gimpactData->m_localScaling);
		//	gimpactShape->setLocalScaling(localScaling);
		//	gimpactShape->setMargin(btScalar(gimpactData->m_collisionMargin));
		//	gimpactShape->updateBound();
		//	shape = gimpactShape;
		//}
		//else
		//{
		//	printf("unsupported gimpact sub type\n");
		//}
		break;
	}
	//The btCapsuleShape* API has issue passing the margin/scaling/halfextents unmodified through the API
	//so deal with this
	case CAPSULE_SHAPE_PROXYTYPE:
	{
		btCapsuleShapeData* capData = (btCapsuleShapeData*)shapeData;


		switch (capData->m_upAxis)
		{
		case 0:
		{
			shape = createCapsuleShapeX(1, 1);
			break;
		}
		case 1:
		{
			shape = createCapsuleShapeY(1, 1);
			break;
		}
		case 2:
		{
			shape = createCapsuleShapeZ(1, 1);
			break;
		}
		default:
		{
			printf("error: wrong up axis for btCapsuleShape\n");
		}


		};
		if (shape)
		{
			btCapsuleShape* cap = (btCapsuleShape*)shape;
			cap->deSerializeFloat(capData);
		}
		break;
	}
	case CYLINDER_SHAPE_PROXYTYPE:
	case CONE_SHAPE_PROXYTYPE:
	case BOX_SHAPE_PROXYTYPE:
	case SPHERE_SHAPE_PROXYTYPE:
	case MULTI_SPHERE_SHAPE_PROXYTYPE:
	case CONVEX_HULL_SHAPE_PROXYTYPE:
	{
		btConvexInternalShapeData* bsd = (btConvexInternalShapeData*)shapeData;
		btVector3 implicitShapeDimensions;
		implicitShapeDimensions.deSerializeFloat(bsd->m_implicitShapeDimensions);
		btVector3 localScaling;
		localScaling.deSerializeFloat(bsd->m_localScaling);
		btVector3 margin(bsd->m_collisionMargin, bsd->m_collisionMargin, bsd->m_collisionMargin);
		switch (shapeData->m_shapeType)
		{
		case BOX_SHAPE_PROXYTYPE:
		{
			btBoxShape* box = (btBoxShape*)createBoxShape(implicitShapeDimensions / localScaling + margin);
			//box->initializePolyhedralFeatures();
			shape = box;

			break;
		}
		case SPHERE_SHAPE_PROXYTYPE:
		{
			shape = createSphereShape(implicitShapeDimensions.getX());
			break;
		}

		case CYLINDER_SHAPE_PROXYTYPE:
		{
			btCylinderShapeData* cylData = (btCylinderShapeData*)shapeData;
			btVector3 halfExtents = implicitShapeDimensions + margin;
			switch (cylData->m_upAxis)
			{
			case 0:
			{
				shape = createCylinderShapeX(halfExtents.getY(), halfExtents.getX());
				break;
			}
			case 1:
			{
				shape = createCylinderShapeY(halfExtents.getX(), halfExtents.getY());
				break;
			}
			case 2:
			{
				shape = createCylinderShapeZ(halfExtents.getX(), halfExtents.getZ());
				break;
			}
			default:
			{
				printf("unknown Cylinder up axis\n");
			}

			};



			break;
		}
		case CONE_SHAPE_PROXYTYPE:
		{
			btConeShapeData* conData = (btConeShapeData*)shapeData;
			btVector3 halfExtents = implicitShapeDimensions;//+margin;
			switch (conData->m_upIndex)
			{
			case 0:
			{
				shape = createConeShapeX(halfExtents.getY(), halfExtents.getX());
				break;
			}
			case 1:
			{
				shape = createConeShapeY(halfExtents.getX(), halfExtents.getY());
				break;
			}
			case 2:
			{
				shape = createConeShapeZ(halfExtents.getX(), halfExtents.getZ());
				break;
			}
			default:
			{
				printf("unknown Cone up axis\n");
			}

			};



			break;
		}
		case MULTI_SPHERE_SHAPE_PROXYTYPE:
		{
			btMultiSphereShapeData* mss = (btMultiSphereShapeData*)bsd;
			int numSpheres = mss->m_localPositionArraySize;

			btAlignedObjectArray<btVector3> tmpPos;
			btAlignedObjectArray<btScalar> radii;
			radii.resize(numSpheres);
			tmpPos.resize(numSpheres);
			int i;
			for (i = 0; i<numSpheres; i++)
			{
				tmpPos[i].deSerializeFloat(mss->m_localPositionArrayPtr[i].m_pos);
				radii[i] = mss->m_localPositionArrayPtr[i].m_radius;
			}
			shape = createMultiSphereShape(&tmpPos[0], &radii[0], numSpheres);
			break;
		}
		case CONVEX_HULL_SHAPE_PROXYTYPE:
		{
			//	int sz = sizeof(btConvexHullShapeData);
			//	int sz2 = sizeof(btConvexInternalShapeData);
			//	int sz3 = sizeof(btCollisionShapeData);
			btConvexHullShapeData* convexData = (btConvexHullShapeData*)bsd;
			int numPoints = convexData->m_numUnscaledPoints;

			btAlignedObjectArray<btVector3> tmpPoints;
			tmpPoints.resize(numPoints);
			int i;
			for (i = 0; i<numPoints; i++)
			{
#ifdef BT_USE_DOUBLE_PRECISION
				if (convexData->m_unscaledPointsDoublePtr)
					tmpPoints[i].deSerialize(convexData->m_unscaledPointsDoublePtr[i]);
				if (convexData->m_unscaledPointsFloatPtr)
					tmpPoints[i].deSerializeFloat(convexData->m_unscaledPointsFloatPtr[i]);
#else
				if (convexData->m_unscaledPointsFloatPtr)
					tmpPoints[i].deSerialize(convexData->m_unscaledPointsFloatPtr[i]);
				if (convexData->m_unscaledPointsDoublePtr)
					tmpPoints[i].deSerializeDouble(convexData->m_unscaledPointsDoublePtr[i]);
#endif //BT_USE_DOUBLE_PRECISION
			}
			btConvexHullShape* hullShape = createConvexHullShape();
			for (i = 0; i<numPoints; i++)
			{
				hullShape->addPoint(tmpPoints[i]);
			}
			hullShape->setMargin(bsd->m_collisionMargin);
			//hullShape->initializePolyhedralFeatures();
			shape = hullShape;
			break;
		}
		default:
		{
			printf("error: cannot create shape type (%d)\n", shapeData->m_shapeType);
		}
		}

		if (shape)
		{
			shape->setMargin(bsd->m_collisionMargin);

			btVector3 localScaling;
			localScaling.deSerializeFloat(bsd->m_localScaling);
			shape->setLocalScaling(localScaling);

		}
		break;
	}
	case TRIANGLE_MESH_SHAPE_PROXYTYPE:
	{
//		btTriangleMeshShapeData* trimesh = (btTriangleMeshShapeData*)shapeData;
//		btStridingMeshInterfaceData* interfaceData = createStridingMeshInterfaceData(&trimesh->m_meshInterface);
//		btTriangleIndexVertexArray* meshInterface = createMeshInterface(*interfaceData);
//		if (!meshInterface->getNumSubParts())
//		{
//			return 0;
//		}
//
//		btVector3 scaling; scaling.deSerializeFloat(trimesh->m_meshInterface.m_scaling);
//		meshInterface->setScaling(scaling);
//
//
//		btOptimizedBvh* bvh = 0;
//#if 1
//		if (trimesh->m_quantizedFloatBvh)
//		{
//			btOptimizedBvh** bvhPtr = m_bvhMap.find(trimesh->m_quantizedFloatBvh);
//			if (bvhPtr && *bvhPtr)
//			{
//				bvh = *bvhPtr;
//			}
//			else
//			{
//				bvh = createOptimizedBvh();
//				bvh->deSerializeFloat(*trimesh->m_quantizedFloatBvh);
//			}
//		}
//		if (trimesh->m_quantizedDoubleBvh)
//		{
//			btOptimizedBvh** bvhPtr = m_bvhMap.find(trimesh->m_quantizedDoubleBvh);
//			if (bvhPtr && *bvhPtr)
//			{
//				bvh = *bvhPtr;
//			}
//			else
//			{
//				bvh = createOptimizedBvh();
//				bvh->deSerializeDouble(*trimesh->m_quantizedDoubleBvh);
//			}
//		}
//#endif
//
//
//		btBvhTriangleMeshShape* trimeshShape = createBvhTriangleMeshShape(meshInterface, bvh);
//		trimeshShape->setMargin(trimesh->m_collisionMargin);
//		shape = trimeshShape;
//
//		if (trimesh->m_triangleInfoMap)
//		{
//			btTriangleInfoMap* map = createTriangleInfoMap();
//			map->deSerialize(*trimesh->m_triangleInfoMap);
//			trimeshShape->setTriangleInfoMap(map);
//
//#ifdef USE_INTERNAL_EDGE_UTILITY
//			gContactAddedCallback = btAdjustInternalEdgeContactsCallback;
//#endif //USE_INTERNAL_EDGE_UTILITY
//
//		}
//
//		//printf("trimesh->m_collisionMargin=%f\n",trimesh->m_collisionMargin);
		break;
	}
	case COMPOUND_SHAPE_PROXYTYPE:
	{
		btCompoundShapeData* compoundData = (btCompoundShapeData*)shapeData;
		btCompoundShape* compoundShape = createCompoundShape();

		btCompoundShapeChildData* childShapeDataArray = &compoundData->m_childShapePtr[0];


		btAlignedObjectArray<btCollisionShape*> childShapes;
		for (int i = 0; i<compoundData->m_numChildShapes; i++)
		{
			btCompoundShapeChildData* ptr = &compoundData->m_childShapePtr[i];

			btCollisionShapeData* cd = compoundData->m_childShapePtr[i].m_childShape;

			btCollisionShape* childShape = convertCollisionShape(cd);
			if (childShape)
			{
				btTransform localTransform;
				localTransform.deSerializeFloat(compoundData->m_childShapePtr[i].m_transform);
				compoundShape->addChildShape(localTransform, childShape);
			}
			else
			{
#ifdef _DEBUG
				printf("error: couldn't create childShape for compoundShape\n");
#endif
			}

		}
		shape = compoundShape;

		break;
	}
	case SOFTBODY_SHAPE_PROXYTYPE:
	{
		return 0;
	}
	default:
	{
#ifdef _DEBUG
		printf("unsupported shape type (%d)\n", shapeData->m_shapeType);
#endif
	}
	}

	return shape;

}

btCollisionShape* ReshapeableRigidBodySection::createPlaneShape(const btVector3& planeNormal, btScalar planeConstant)
{
	btStaticPlaneShape* shape = new btStaticPlaneShape(planeNormal, planeConstant);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createBoxShape(const btVector3& halfExtents)
{
	btBoxShape* shape = new btBoxShape(halfExtents);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createSphereShape(btScalar radius)
{
	btSphereShape* shape = new btSphereShape(radius);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createCapsuleShapeX(btScalar radius, btScalar height)
{
	btCapsuleShapeX* shape = new btCapsuleShapeX(radius, height);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createCapsuleShapeY(btScalar radius, btScalar height)
{
	btCapsuleShape* shape = new btCapsuleShape(radius, height);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createCapsuleShapeZ(btScalar radius, btScalar height)
{
	btCapsuleShapeZ* shape = new btCapsuleShapeZ(radius, height);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createCylinderShapeX(btScalar radius, btScalar height)
{
	btCylinderShapeX* shape = new btCylinderShapeX(btVector3(height, radius, radius));
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createCylinderShapeY(btScalar radius, btScalar height)
{
	btCylinderShape* shape = new btCylinderShape(btVector3(radius, height, radius));
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createCylinderShapeZ(btScalar radius, btScalar height)
{
	btCylinderShapeZ* shape = new btCylinderShapeZ(btVector3(radius, radius, height));
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createConeShapeX(btScalar radius, btScalar height)
{
	btConeShapeX* shape = new btConeShapeX(radius, height);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createConeShapeY(btScalar radius, btScalar height)
{
	btConeShape* shape = new btConeShape(radius, height);
	return shape;
}

btCollisionShape* ReshapeableRigidBodySection::createConeShapeZ(btScalar radius, btScalar height)
{
	btConeShapeZ* shape = new btConeShapeZ(radius, height);
	return shape;
}

btTriangleIndexVertexArray*	ReshapeableRigidBodySection::createTriangleMeshContainer()
{
	btTriangleIndexVertexArray* in = new btTriangleIndexVertexArray();
	return in;
}

btOptimizedBvh*	ReshapeableRigidBodySection::createOptimizedBvh()
{
	btOptimizedBvh* bvh = new btOptimizedBvh();
	return bvh;
}


btTriangleInfoMap* ReshapeableRigidBodySection::createTriangleInfoMap()
{
	btTriangleInfoMap* tim = new btTriangleInfoMap();
	return tim;
}

btBvhTriangleMeshShape* ReshapeableRigidBodySection::createBvhTriangleMeshShape(btStridingMeshInterface* trimesh, btOptimizedBvh* bvh)
{
	if (bvh)
	{
		btBvhTriangleMeshShape* bvhTriMesh = new btBvhTriangleMeshShape(trimesh, bvh->isQuantized(), false);
		bvhTriMesh->setOptimizedBvh(bvh);
		return bvhTriMesh;
	}

	btBvhTriangleMeshShape* ts = new btBvhTriangleMeshShape(trimesh, true);
	return ts;

}
btCollisionShape* ReshapeableRigidBodySection::createConvexTriangleMeshShape(btStridingMeshInterface* trimesh)
{
	return 0;
}

btConvexHullShape* ReshapeableRigidBodySection::createConvexHullShape()
{
	btConvexHullShape* shape = new btConvexHullShape();
	return shape;
}

btCompoundShape* ReshapeableRigidBodySection::createCompoundShape()
{
	btCompoundShape* shape = new btCompoundShape();
	return shape;
}


btScaledBvhTriangleMeshShape* ReshapeableRigidBodySection::createScaledTrangleMeshShape(btBvhTriangleMeshShape* meshShape, const btVector3& localScaling)
{
	btScaledBvhTriangleMeshShape* shape = new btScaledBvhTriangleMeshShape(meshShape, localScaling);
	return shape;
}

btMultiSphereShape* ReshapeableRigidBodySection::createMultiSphereShape(const btVector3* positions, const btScalar* radi, int numSpheres)
{
	btMultiSphereShape* shape = new btMultiSphereShape(positions, radi, numSpheres);
	return shape;
}