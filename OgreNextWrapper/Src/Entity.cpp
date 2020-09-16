#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::Mesh* Entity_getMesh(Ogre::v1::Entity* entity, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::v1::MeshPtr& meshPtr = entity->getMesh();
	processWrapper(meshPtr.getPointer(), &meshPtr);
	return meshPtr.getPointer();
}

extern "C" _AnomalousExport Ogre::v1::SubEntity* Entity_getSubEntityIndex(Ogre::v1::Entity* entity, uint index)
{
	return entity->getSubEntity(index);
}

extern "C" _AnomalousExport Ogre::v1::SubEntity* Entity_getSubEntity(Ogre::v1::Entity* entity, const char* name)
{
	return entity->getSubEntity(name);
}

extern "C" _AnomalousExport uint Entity_getNumSubEntities(Ogre::v1::Entity* entity)
{
	return entity->getNumSubEntities();
}

extern "C" _AnomalousExport void Entity_setMaterialName(Ogre::v1::Entity* entity, const char* name)
{
	entity->setMaterialName(name);
}

extern "C" _AnomalousExport Ogre::v1::AnimationState* Entity_getAnimationState(Ogre::v1::Entity* entity, const char* name)
{
	return entity->getAnimationState(name);
}

extern "C" _AnomalousExport Ogre::v1::AnimationStateSet* Entity_getAllAnimationStates(Ogre::v1::Entity* entity)
{
	return entity->getAllAnimationStates();
}

extern "C" _AnomalousExport void Entity_setDisplaySkeleton(Ogre::v1::Entity* entity, bool display)
{
	entity->setDisplaySkeleton(display);
}

extern "C" _AnomalousExport bool Entity_getDisplaySkeleton(Ogre::v1::Entity* entity)
{
	return entity->getDisplaySkeleton();
}

extern "C" _AnomalousExport Ogre::v1::Entity* Entity_getManualLodLevel(Ogre::v1::Entity* entity, uint index)
{
	return entity->getManualLodLevel(index);
}

extern "C" _AnomalousExport uint Entity_getNumManualLodLevels(Ogre::v1::Entity* entity)
{
	return entity->getNumManualLodLevels();
}

extern "C" _AnomalousExport ushort Entity_getCurrentLodIndex(Ogre::v1::Entity* entity)
{
	return entity->getCurrentLodIndex();
}

extern "C" _AnomalousExport void Entity_setMeshLodBias(Ogre::v1::Entity* entity, float factor, ushort maxDetailIndex, ushort minDetailIndex)
{
	entity->setMeshLodBias(factor, maxDetailIndex, minDetailIndex);
}

extern "C" _AnomalousExport void Entity_setMaterialLodBias(Ogre::v1::Entity* entity, float factor, ushort maxDetailIndex, ushort minDetailIndex)
{
	entity->setMaterialLodBias(factor, maxDetailIndex, minDetailIndex);
}

extern "C" _AnomalousExport void Entity_setPolygonModeOverrideable(Ogre::v1::Entity* entity, bool polygonModeOverrideable)
{
	entity->setPolygonModeOverrideable(polygonModeOverrideable);
}

extern "C" _AnomalousExport void Entity_detachAllObjectsFromBone(Ogre::v1::Entity* entity)
{
	entity->detachAllObjectsFromBone();
}

extern "C" _AnomalousExport float Entity_getBoundingRadius(Ogre::v1::Entity* entity)
{
	return entity->getBoundingRadius();
}

extern "C" _AnomalousExport bool Entity_hasSkeleton(Ogre::v1::Entity* entity)
{
	return entity->hasSkeleton();
}

extern "C" _AnomalousExport Ogre::v1::OldSkeletonInstance* Entity_getSkeleton(Ogre::v1::Entity* entity)
{
	return entity->getSkeleton();
}

extern "C" _AnomalousExport bool Entity_isHardwareAnimationEnabled(Ogre::v1::Entity* entity)
{
	return entity->isHardwareAnimationEnabled();
}

extern "C" _AnomalousExport int Entity_getSoftwareAnimationRequests(Ogre::v1::Entity* entity)
{
	return entity->getSoftwareAnimationRequests();
}

extern "C" _AnomalousExport int Entity_getSoftwareAnimationNormalsRequests(Ogre::v1::Entity* entity)
{
	return entity->getSoftwareAnimationNormalsRequests();
}

extern "C" _AnomalousExport void Entity_addSoftwareAnimationRequest(Ogre::v1::Entity* entity, bool normalsAlso)
{
	entity->addSoftwareAnimationRequest(normalsAlso);
}

extern "C" _AnomalousExport void Entity_removeSoftwareAnimationRequest(Ogre::v1::Entity* entity, bool normalsAlso)
{
	entity->removeSoftwareAnimationRequest(normalsAlso);
}

extern "C" _AnomalousExport void Entity_shareSkeletonInstanceWith(Ogre::v1::Entity* entity, Ogre::v1::Entity* shareEntity)
{
	entity->shareSkeletonInstanceWith(shareEntity);
}

extern "C" _AnomalousExport bool Entity_hasVertexAnimation(Ogre::v1::Entity* entity)
{
	return entity->hasVertexAnimation();
}

extern "C" _AnomalousExport void Entity_stopSharingSkeletonInstance(Ogre::v1::Entity* entity)
{
	entity->stopSharingSkeletonInstance();
}

extern "C" _AnomalousExport bool Entity_sharesSkeletonInstance(Ogre::v1::Entity* entity)
{
	return entity->sharesSkeletonInstance();
}

extern "C" _AnomalousExport void Entity_refreshAvailableAnimationState(Ogre::v1::Entity* entity)
{
	return entity->refreshAvailableAnimationState();
}

extern "C" _AnomalousExport bool Entity_isInitialzed(Ogre::v1::Entity* entity)
{
	return entity->isInitialised();
}

extern "C" _AnomalousExport void Entity_setCastShadows(Ogre::v1::Entity* entity, bool castShadows)
{
	entity->setCastShadows(castShadows);
}

extern "C" _AnomalousExport bool Entity_getCastShadows(Ogre::v1::Entity* entity)
{
	return entity->getCastShadows();
}

extern "C" _AnomalousExport AxisAlignedBox Entity_getChildObjectsBoundingBox(Ogre::v1::Entity* entity)
{
	return entity->getChildObjectsBoundingBox();
}

extern "C" _AnomalousExport void Entity_animateVertexData(Ogre::v1::Entity* entity, Ogre::v1::VertexData* vertexData, ushort subEntityIndex)
{
	const Ogre::Matrix4* blendMatrices[256];

	Ogre::v1::MeshPtr mesh = entity->getMesh();
	Ogre::v1::SubMesh* subMesh = mesh->getSubMesh(subEntityIndex);

	Ogre::v1::Mesh::prepareMatricesForVertexBlend(blendMatrices, entity->_getBoneMatrices(), subMesh->blendIndexToBoneIndexMap);

	Ogre::v1::Mesh::softwareVertexBlend(
		subMesh->vertexData,
		vertexData,
		blendMatrices, 
		mesh->sharedBlendIndexToBoneIndexMap.size(),
		false);
}

float computeRayIntersectTriDistance(Ogre::Ray ray, Ogre::Vector3 vert0, Ogre::Vector3 vert1, Ogre::Vector3 vert2);
void getMeshInformation(Ogre::v1::Entity* entity, size_t &vertexCount, Ogre::Vector3* &vertices,
								size_t &indexCount, unsigned long* &indices,
								const Ogre::Vector3 &position, const Ogre::Quaternion &orient,
								const Ogre::Vector3 &scale);

//Thanks to http://www.ogre3d.org/tikiwiki/tiki-index.php?page=Raycasting+to+the+polygon+level for this.
extern "C" _AnomalousExport bool Entity_raycastPolygonLevel(Ogre::v1::Entity* entity, Ray3 ray, float& distanceOnRay)
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

void getMeshInformation(Ogre::v1::Entity* entity, size_t &vertexCount, Ogre::Vector3* &vertices,
								size_t &indexCount, unsigned long* &indices,
								const Ogre::Vector3 &position, const Ogre::Quaternion &orient,
								const Ogre::Vector3 &scale)
{
	Ogre::v1::MeshPtr mesh = entity->getMesh();

	bool added_shared = false;
	size_t current_offset = 0;
	size_t shared_offset = 0;
	size_t next_offset = 0;
	size_t index_offset = 0;

	vertexCount = indexCount = 0;

	// Calculate how many vertices and indices we're going to need
	for (unsigned short i = 0; i < mesh->getNumSubMeshes(); ++i)
	{
		Ogre::v1::SubMesh* submesh = mesh->getSubMesh( i );

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
		Ogre::v1::SubMesh* submesh = mesh->getSubMesh(i);

		Ogre::v1::VertexData* vertex_data = submesh->useSharedVertices ? mesh->sharedVertexData : submesh->vertexData;

		if((!submesh->useSharedVertices)||(submesh->useSharedVertices && !added_shared))
		{
			if(submesh->useSharedVertices)
			{
				added_shared = true;
				shared_offset = current_offset;
			}

			const Ogre::v1::VertexElement* posElem =
				vertex_data->vertexDeclaration->findElementBySemantic(Ogre::VES_POSITION);

			Ogre::v1::HardwareVertexBufferSharedPtr vbuf =
				vertex_data->vertexBufferBinding->getBuffer(posElem->getSource());

			unsigned char* vertex =
				static_cast<unsigned char*>(vbuf->lock(Ogre::v1::HardwareBuffer::HBL_READ_ONLY));

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


		Ogre::v1::IndexData* index_data = submesh->indexData;
		size_t numTris = index_data->indexCount / 3;
		Ogre::v1::HardwareIndexBufferSharedPtr ibuf = index_data->indexBuffer;

		bool use32bitindexes = (ibuf->getType() == Ogre::v1::HardwareIndexBuffer::IT_32BIT);

		unsigned long*  pLong = static_cast<unsigned long*>(ibuf->lock(Ogre::v1::HardwareBuffer::HBL_READ_ONLY));
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