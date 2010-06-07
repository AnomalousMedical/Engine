#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::Mesh* Entity_getMesh(Ogre::Entity* entity, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::MeshPtr& meshPtr = entity->getMesh();
	processWrapper(meshPtr.getPointer(), &meshPtr);
	return meshPtr.getPointer();
}

extern "C" _AnomalousExport Ogre::SubEntity* Entity_getSubEntityIndex(Ogre::Entity* entity, uint index)
{
	return entity->getSubEntity(index);
}

extern "C" _AnomalousExport Ogre::SubEntity* Entity_getSubEntity(Ogre::Entity* entity, const char* name)
{
	return entity->getSubEntity(name);
}

extern "C" _AnomalousExport uint Entity_getNumSubEntities(Ogre::Entity* entity)
{
	return entity->getNumSubEntities();
}

extern "C" _AnomalousExport void Entity_setMaterialName(Ogre::Entity* entity, const char* name)
{
	entity->setMaterialName(name);
}

extern "C" _AnomalousExport Ogre::AnimationState* Entity_getAnimationState(Ogre::Entity* entity, const char* name)
{
	return entity->getAnimationState(name);
}

extern "C" _AnomalousExport Ogre::AnimationStateSet* Entity_getAllAnimationStates(Ogre::Entity* entity)
{
	return entity->getAllAnimationStates();
}

extern "C" _AnomalousExport void Entity_setDisplaySkeleton(Ogre::Entity* entity, bool display)
{
	entity->setDisplaySkeleton(display);
}

extern "C" _AnomalousExport bool Entity_getDisplaySkeleton(Ogre::Entity* entity)
{
	return entity->getDisplaySkeleton();
}

extern "C" _AnomalousExport Ogre::Entity* Entity_getManualLodLevel(Ogre::Entity* entity, uint index)
{
	return entity->getManualLodLevel(index);
}

extern "C" _AnomalousExport uint Entity_getNumManualLodLevels(Ogre::Entity* entity)
{
	return entity->getNumManualLodLevels();
}

extern "C" _AnomalousExport ushort Entity_getCurrentLodIndex(Ogre::Entity* entity)
{
	return entity->getCurrentLodIndex();
}

extern "C" _AnomalousExport void Entity_setMeshLodBias(Ogre::Entity* entity, float factor, ushort maxDetailIndex, ushort minDetailIndex)
{
	entity->setMeshLodBias(factor, maxDetailIndex, minDetailIndex);
}

extern "C" _AnomalousExport void Entity_setMaterialLodBias(Ogre::Entity* entity, float factor, ushort maxDetailIndex, ushort minDetailIndex)
{
	entity->setMaterialLodBias(factor, maxDetailIndex, minDetailIndex);
}

extern "C" _AnomalousExport void Entity_setPolygonModeOverrideable(Ogre::Entity* entity, bool polygonModeOverrideable)
{
	entity->setPolygonModeOverrideable(polygonModeOverrideable);
}

extern "C" _AnomalousExport void Entity_detachAllObjectsFromBone(Ogre::Entity* entity)
{
	entity->detachAllObjectsFromBone();
}

extern "C" _AnomalousExport float Entity_getBoundingRadius(Ogre::Entity* entity)
{
	return entity->getBoundingRadius();
}

extern "C" _AnomalousExport bool Entity_hasSkeleton(Ogre::Entity* entity)
{
	return entity->hasSkeleton();
}

extern "C" _AnomalousExport Ogre::SkeletonInstance* Entity_getSkeleton(Ogre::Entity* entity)
{
	return entity->getSkeleton();
}

extern "C" _AnomalousExport bool Entity_isHardwareAnimationEnabled(Ogre::Entity* entity)
{
	return entity->isHardwareAnimationEnabled();
}

extern "C" _AnomalousExport int Entity_getSoftwareAnimationRequests(Ogre::Entity* entity)
{
	return entity->getSoftwareAnimationRequests();
}

extern "C" _AnomalousExport int Entity_getSoftwareAnimationNormalsRequests(Ogre::Entity* entity)
{
	return entity->getSoftwareAnimationNormalsRequests();
}

extern "C" _AnomalousExport void Entity_addSoftwareAnimationRequest(Ogre::Entity* entity, bool normalsAlso)
{
	entity->addSoftwareAnimationRequest(normalsAlso);
}

extern "C" _AnomalousExport void Entity_removeSoftwareAnimationRequest(Ogre::Entity* entity, bool normalsAlso)
{
	entity->removeSoftwareAnimationRequest(normalsAlso);
}

extern "C" _AnomalousExport void Entity_shareSkeletonInstanceWith(Ogre::Entity* entity, Ogre::Entity* shareEntity)
{
	entity->shareSkeletonInstanceWith(shareEntity);
}

extern "C" _AnomalousExport bool Entity_hasVertexAnimation(Ogre::Entity* entity)
{
	return entity->hasVertexAnimation();
}

extern "C" _AnomalousExport void Entity_stopSharingSkeletonInstance(Ogre::Entity* entity)
{
	entity->stopSharingSkeletonInstance();
}

extern "C" _AnomalousExport bool Entity_sharesSkeletonInstance(Ogre::Entity* entity)
{
	return entity->sharesSkeletonInstance();
}

extern "C" _AnomalousExport void Entity_refreshAvailableAnimationState(Ogre::Entity* entity)
{
	return entity->refreshAvailableAnimationState();
}

extern "C" _AnomalousExport bool Entity_isInitialzed(Ogre::Entity* entity)
{
	return entity->isInitialised();
}

extern "C" _AnomalousExport void Entity_setCastShadows(Ogre::Entity* entity, bool castShadows)
{
	entity->setCastShadows(castShadows);
}

extern "C" _AnomalousExport bool Entity_getCastShadows(Ogre::Entity* entity)
{
	return entity->getCastShadows();
}

float computeRayIntersectTriDistance(Ogre::Ray ray, Ogre::Vector3 vert0, Ogre::Vector3 vert1, Ogre::Vector3 vert2);
void getMeshInformation(Ogre::Entity* entity, size_t &vertexCount, Ogre::Vector3* &vertices,
								size_t &indexCount, unsigned long* &indices,
								const Ogre::Vector3 &position, const Ogre::Quaternion &orient,
								const Ogre::Vector3 &scale);

extern "C" _AnomalousExport bool Entity_raycastPolygonLevel(Ogre::Entity* entity, Ray3 ray, float distanceOnRay)
{
	Ogre::Real closestDistance = -1.0f;
	Ogre::Ray ogreRay = ray.toOgre();

	// mesh data to retrieve         
	size_t vertexCount;
	size_t indexCount;
	Ogre::Vector3 *vertices;
	unsigned long *indices;

	// get the mesh information
	getMeshInformation(entity, vertexCount, vertices, indexCount, indices,             
		entity->getParentNode()->_getDerivedPosition(),
		entity->getParentNode()->_getDerivedOrientation(),
		entity->getParentNode()->_getDerivedScale());

	// test for hitting individual triangles on the mesh
	bool hitMesh = false;
	int hitBaseVertex = 0;
	for (int i = 0; i < static_cast<int>(indexCount); i += 3)
	{
		// check for a hit against this triangle
		std::pair<bool, Ogre::Real> hit = Ogre::Math::intersects(ogreRay, vertices[indices[i]],
			vertices[indices[i+1]], vertices[indices[i+2]], true, false);

		// if it was a hit check if its the closest
		if (hit.first)
		{
			if ((closestDistance < 0.0f) || (hit.second < closestDistance))
			{
				// this is the closest so far, save it off
				closestDistance = hit.second;
				hitMesh = true;
				hitBaseVertex = i;
			}
		}
	}

	//If we found something put it into distanceOnRay.
	if (hitMesh)
	{
		distanceOnRay = computeRayIntersectTriDistance( ogreRay,
			vertices[indices[hitBaseVertex]], 
			vertices[indices[hitBaseVertex + 1]],
			vertices[indices[hitBaseVertex + 2]]);
	}

	// free the verticies and indicies memory
	delete[] vertices;
	delete[] indices;

	return hitMesh;
}

void getMeshInformation(Ogre::Entity* entity, size_t &vertexCount, Ogre::Vector3* &vertices,
								size_t &indexCount, unsigned long* &indices,
								const Ogre::Vector3 &position, const Ogre::Quaternion &orient,
								const Ogre::Vector3 &scale)
{
	Ogre::MeshPtr mesh = entity->getMesh();

	bool added_shared = false;
	size_t current_offset = 0;
	size_t shared_offset = 0;
	size_t next_offset = 0;
	size_t index_offset = 0;

	vertexCount = indexCount = 0;

	// Calculate how many vertices and indices we're going to need
	for (unsigned short i = 0; i < mesh->getNumSubMeshes(); ++i)
	{
		Ogre::SubMesh* submesh = mesh->getSubMesh( i );

		// We only need to add the shared vertices once
		if(submesh->useSharedVertices)
		{
			if( !added_shared )
			{
				vertexCount += mesh->sharedVertexData->vertexCount;
				added_shared = true;
			}
		}
		else
		{
			vertexCount += submesh->vertexData->vertexCount;
		}

		// Add the indices
		indexCount += submesh->indexData->indexCount;
	}


	// Allocate space for the vertices and indices
	vertices = new Ogre::Vector3[vertexCount];
	indices = new unsigned long[indexCount];

	added_shared = false;

	// Run through the submeshes again, adding the data into the arrays
	for ( unsigned short i = 0; i < mesh->getNumSubMeshes(); ++i)
	{
		Ogre::SubMesh* submesh = mesh->getSubMesh(i);

		Ogre::VertexData* vertex_data = submesh->useSharedVertices ? mesh->sharedVertexData : submesh->vertexData;

		if((!submesh->useSharedVertices)||(submesh->useSharedVertices && !added_shared))
		{
			if(submesh->useSharedVertices)
			{
				added_shared = true;
				shared_offset = current_offset;
			}

			const Ogre::VertexElement* posElem =
				vertex_data->vertexDeclaration->findElementBySemantic(Ogre::VES_POSITION);

			Ogre::HardwareVertexBufferSharedPtr vbuf =
				vertex_data->vertexBufferBinding->getBuffer(posElem->getSource());

			unsigned char* vertex =
				static_cast<unsigned char*>(vbuf->lock(Ogre::HardwareBuffer::HBL_READ_ONLY));

			// There is _no_ baseVertexPointerToElement() which takes an Ogre::Real or a double
			//  as second argument. So make it float, to avoid trouble when Ogre::Real will
			//  be comiled/typedefed as double:
			//      Ogre::Real* pReal;
			float* pReal;

			for( size_t j = 0; j < vertex_data->vertexCount; ++j, vertex += vbuf->getVertexSize())
			{
				posElem->baseVertexPointerToElement(vertex, &pReal);

				Ogre::Vector3 pt(pReal[0], pReal[1], pReal[2]);

				vertices[current_offset + j] = (orient * (pt * scale)) + position;
			}

			vbuf->unlock();
			next_offset += vertex_data->vertexCount;
		}


		Ogre::IndexData* index_data = submesh->indexData;
		size_t numTris = index_data->indexCount / 3;
		Ogre::HardwareIndexBufferSharedPtr ibuf = index_data->indexBuffer;

		bool use32bitindexes = (ibuf->getType() == Ogre::HardwareIndexBuffer::IT_32BIT);

		unsigned long*  pLong = static_cast<unsigned long*>(ibuf->lock(Ogre::HardwareBuffer::HBL_READ_ONLY));
		unsigned short* pShort = reinterpret_cast<unsigned short*>(pLong);


		size_t offset = (submesh->useSharedVertices)? shared_offset : current_offset;

		if ( use32bitindexes )
		{
			for ( size_t k = 0; k < numTris*3; ++k)
			{
				indices[index_offset++] = pLong[k] + static_cast<unsigned long>(offset);
			}
		}
		else
		{
			for ( size_t k = 0; k < numTris*3; ++k)
			{
				indices[index_offset++] = static_cast<unsigned long>(pShort[k]) +
					static_cast<unsigned long>(offset);
			}
		}

		ibuf->unlock();
		current_offset = next_offset;
	}
}

//Helper function, finds the distance along the given ray that intersects the given triangle.
//This function assumes that the two intersect.
float computeRayIntersectTriDistance(Ogre::Ray ray, Ogre::Vector3 vert0, Ogre::Vector3 vert1, Ogre::Vector3 vert2)
{
	Ogre::Vector3 edge1 = vert1 - vert0;
	Ogre::Vector3 edge2 = vert2 - vert0;

	Ogre::Vector3 pvec = ray.getDirection().crossProduct(edge2);
	float inv_det = 1.0f / edge1.dotProduct(pvec);
	Ogre::Vector3 tvec = ray.getOrigin() - vert0;
	Ogre::Vector3 qvec = tvec.crossProduct(edge1);

	return edge2.dotProduct(qvec) * inv_det;
}