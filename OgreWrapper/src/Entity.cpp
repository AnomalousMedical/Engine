/// <file>Entity.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Entity.h"
#include "MarshalUtils.h"
#include "SubEntity.h"
#include "SkeletonInstance.h"
#include "AnimationState.h"
#include "AnimationStateSet.h"
#include "MathUtils.h"
#include "MeshManager.h"

#include "Ogre.h"

namespace OgreWrapper{

Entity::Entity(Ogre::Entity* entity)
:MovableObject(entity),
entity( entity ),
skeleton(nullptr),
animationStateSet(nullptr), 
root(new RenderEntityGCRoot())
{
	*(root.Get()) = this;
	userDefinedObj.Reset(new VoidUserDefinedObject(RENDER_ENTITY_GCROOT, root.Get()));
	entity->setUserObject(userDefinedObj.Get());
}

Entity::~Entity()
{
	delete skeleton;
}

Ogre::Entity* Entity::getEntity()
{
	return entity;
}

System::String^ Entity::getName()
{
	return MarshalUtils::convertString(entity->getName());
}

MeshPtr^ Entity::getMesh()
{
	return MeshManager::getInstance()->getObject(entity->getMesh());
}

SubEntity^ Entity::getSubEntity(unsigned int index)
{
	Ogre::SubEntity* ogreSubEntity = entity->getSubEntity(index);
	return subEntities.getObject(ogreSubEntity);
}

SubEntity^ Entity::getSubEntity(System::String^ name)
{
	Ogre::SubEntity* ogreSubEntity = entity->getSubEntity(MarshalUtils::convertString(name));
	return subEntities.getObject(ogreSubEntity);
}

unsigned int Entity::getNumSubEntities()
{
	return entity->getNumSubEntities();
}

void Entity::setMaterialName(System::String^ name)
{
	entity->setMaterialName(MarshalUtils::convertString(name));
}

AnimationState^ Entity::getAnimationState(System::String^ name)
{
	if(animationStateSet == nullptr)
	{
		animationStateSet = gcnew AnimationStateSet(entity->getAllAnimationStates());
	}
	return animationStateSet->getAnimationState(name);
}

AnimationStateSet^ Entity::getAllAnimationStates()
{
	if(animationStateSet == nullptr)
	{
		animationStateSet = gcnew AnimationStateSet(entity->getAllAnimationStates());
	}
	return animationStateSet;
}

void Entity::setDisplaySkeleton(bool display)
{
	entity->setDisplaySkeleton(display);
}

bool Entity::getDisplaySkeleton()
{
	return entity->getDisplaySkeleton();
}

Entity^ Entity::getManualLodLevel(unsigned int index)
{
	Ogre::Entity* ogreEntity = entity->getManualLodLevel(index);
	return lodEntities.getObject(ogreEntity);
}

unsigned int Entity::getNumManualLodLevels()
{
	return entity->getNumManualLodLevels();
}

unsigned short Entity::getCurrentLodIndex()
{
	return entity->getCurrentLodIndex();
}

void Entity::setMeshLodBias(float factor, unsigned short maxDetailIndex, unsigned short minDetailIndex)
{
	entity->setMeshLodBias(factor, maxDetailIndex, minDetailIndex);
}

void Entity::setMaterialLodBias(float factor, unsigned short maxDetailIndex, unsigned short minDetailIndex)
{
	entity->setMaterialLodBias(factor, maxDetailIndex, minDetailIndex);
}

void Entity::setPolygonModeOverrideable(bool polygonModeOverrideable)
{
	entity->setPolygonModeOverrideable(polygonModeOverrideable);
}

//attach object to bone

//detach object from bone

//detach object from bone

void Entity::detachAllObjectsFromBone()
{
	entity->detachAllObjectsFromBone();
}

//get attached object iterator

float Entity::getBoundingRadius()
{
	return entity->getBoundingRadius();
}

bool Entity::hasSkeleton()
{
	return entity->hasSkeleton();
}

SkeletonInstance^ Entity::getSkeleton()
{
	if(entity->hasSkeleton() && skeleton == nullptr)
	{
		skeleton = gcnew SkeletonInstance(entity->getSkeleton());
	}
	return skeleton;
}

bool Entity::isHardwareAnimationEnabled()
{
	return entity->isHardwareAnimationEnabled();
}

int Entity::getSoftwareAnimationRequests()
{
	return entity->getSoftwareAnimationRequests();
}

int Entity::getSoftwareAnimationNormalsRequests()
{
	return entity->getSoftwareAnimationNormalsRequests();
}

void Entity::addSoftwareAnimationRequest(bool normalsAlso)
{
	entity->addSoftwareAnimationRequest(normalsAlso);
}

void Entity::removeSoftwareAnimationRequest(bool normalsAlso)
{
	entity->removeSoftwareAnimationRequest(normalsAlso);
}

void Entity::shareSkeletonInstanceWith(Entity^ entity)
{
	this->entity->shareSkeletonInstanceWith(entity->entity);
}

bool Entity::hasVertexAnimation()
{
	return entity->hasVertexAnimation();
}

void Entity::stopSharingSkeletonInstance()
{
	entity->stopSharingSkeletonInstance();
}

bool Entity::sharesSkeletonInstance()
{
	return entity->sharesSkeletonInstance();
}

void Entity::refreshAvailableAnimationState()
{
	entity->refreshAvailableAnimationState();
}

bool Entity::isInitialzed()
{
	return entity->isInitialised();
}

void Entity::setCastShadows(bool castShadows)
{
	entity->setCastShadows(castShadows);
}

bool Entity::getCastShadows()
{
	return entity->getCastShadows();
}

void Entity::getMeshInformation(size_t &vertexCount, Ogre::Vector3* &vertices,
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

bool Entity::raycastPolygonLevel(EngineMath::Ray3 ray, float% distanceOnRay)
{
	Ogre::Real closestDistance = -1.0f;
	Ogre::Ray ogreRay = MathUtils::copyRay(ray);

	// mesh data to retrieve         
    size_t vertexCount;
    size_t indexCount;
    Ogre::Vector3 *vertices;
    unsigned long *indices;

    // get the mesh information
	getMeshInformation(vertexCount, vertices, indexCount, indices,             
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

}