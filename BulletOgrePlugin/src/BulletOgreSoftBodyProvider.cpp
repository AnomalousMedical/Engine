#include "StdAfx.h"
#include "BulletOgreSoftBodyProvider.h"
#include "BulletOgreSoftBodyProviderDefinition.h"

#pragma unmanaged
#include "btBulletDynamicsCommon.h"
#include "BulletSoftBody\btSoftRigidDynamicsWorld.h"
#include "BulletSoftBody\btSoftBodyRigidBodyCollisionConfiguration.h"
#include "BulletSoftBody\btSoftBodyHelpers.h"
#pragma managed

namespace BulletOgrePlugin
{

#pragma unmanaged

btSoftBody* createFromTriMesh(btSoftBodyWorldInfo* worldInfo, float* vertices, int* triangles, int nTriangles)
{
	return btSoftBodyHelpers::CreateFromTriMesh(*worldInfo, vertices, triangles, nTriangles);
}

#pragma managed

BulletOgreSoftBodyProvider::BulletOgreSoftBodyProvider(BulletOgreSoftBodyProviderDefinition^ def, OgreSceneManager^ ogreScene)
:SoftBodyProvider(def),
softBody(0),
ogreScene(ogreScene),
meshName(def->MeshName)
{
}

BulletOgreSoftBodyProvider::~BulletOgreSoftBodyProvider(void)
{
}

void BulletOgreSoftBodyProvider::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{
}

void BulletOgreSoftBodyProvider::updateTranslationImpl(Vector3% translation)
{
}

void BulletOgreSoftBodyProvider::updateRotationImpl(Quaternion% rotation)
{
}

void BulletOgreSoftBodyProvider::updateScaleImpl(Vector3% scale)
{
}

void BulletOgreSoftBodyProvider::setEnabled(bool enabled)
{
	
}

void* BulletOgreSoftBodyProvider::createSoftBodyImpl(BulletScene^ scene)
{
	assert(softBody == 0);

	//float center[3] = {0, 0, 0};
	//float radius[3] = {1, 2, 1};
	//softBody = createElipsoid(static_cast<btSoftBodyWorldInfo*>(scene->SoftBodyWorldInfoExternal), center, radius, 512);

	SceneManager^ sceneManager = ogreScene->SceneManager;
	node = sceneManager->createSceneNode(this->Name + "__SoftBodyNode");
	entity = ogreScene->SceneManager->createEntity(this->Name, meshName);
	node->attachObject(entity);
	sceneManager->getRootSceneNode()->addChild(node);

	//Gather data from ogre mesh
	MeshPtr^ meshPtr = entity->getMesh();

    SubMesh^ subMesh = meshPtr->Value->getSubMesh(0);
    VertexData^ vertexData = subMesh->vertexData;
    IndexData^ indexData = subMesh->indexData;
    if (subMesh->UseSharedVertices)
    {
        vertexData = meshPtr->Value->SharedVertexData;
    }

    VertexDeclaration^ vertexDeclaration = vertexData->vertexDeclaration;
	VertexElement^ positionElement = vertexDeclaration->findElementBySemantic(VertexElementSemantic::VES_POSITION);

    VertexBufferBinding^ vertexBinding = vertexData->vertexBufferBinding;
    HardwareVertexBufferSharedPtr^ vertexBuffer = vertexBinding->getBuffer(positionElement->getSource());
    HardwareIndexBufferSharedPtr^ indexBuffer = indexData->IndexBuffer;
    unsigned int vertexSize = vertexBuffer->Value->getVertexSize();
    
    unsigned int numVertices = vertexBuffer->Value->getNumVertices();
    unsigned int positionOffset = positionElement->getOffset();

    unsigned int numIndices = indexBuffer->Value->getNumIndexes();
    unsigned int numTriangles = numIndices / 3;
    bool success = false;

    float* vertices = new float[vertexBuffer->Value->getNumVertices() * 3];
    unsigned int* indices = new unsigned int[indexBuffer->Value->getNumIndexes()];

    // Get vertex data
    Vector3* verticesPtr = (Vector3*)vertices;
	unsigned char* vertexBufferData = (unsigned char*)vertexBuffer->Value->lock(HardwareBuffer::LockOptions::HBL_DISCARD);
    float* elemStart;
    for (unsigned int i = 0; i < numVertices; ++i)
    {
        positionElement->baseVertexPointerToElement(vertexBufferData, &elemStart);
        verticesPtr[i].x = *elemStart++;
        verticesPtr[i].y = *elemStart++;
        verticesPtr[i].z = *elemStart++;
        vertexBufferData += vertexSize;
    }
    vertexBuffer->Value->unlock();
    
    // Get index data
    unsigned int* indicesPtr = (unsigned int*)indices;
	if (indexBuffer->Value->getType() == HardwareIndexBuffer::IndexType::IT_16BIT)
    {
		unsigned short* indexBufferData = (unsigned short*)indexBuffer->Value->lock(HardwareBuffer::LockOptions::HBL_DISCARD);
        for (unsigned int i = 0; i < numIndices; ++i)
        {
            indicesPtr[i] = indexBufferData[i];
        }
        indexBuffer->Value->unlock();
    }
	else if (indexBuffer->Value->getType() == HardwareIndexBuffer::IndexType::IT_32BIT)
    {
		unsigned int* indexBufferData = (unsigned int*)indexBuffer->Value->lock(HardwareBuffer::LockOptions::HBL_DISCARD);
        for (unsigned int i = 0; i < numIndices; ++i)
        {
            indicesPtr[i] = indexBufferData[i];
        }
        indexBuffer->Value->unlock();
    }

	softBody = createFromTriMesh(static_cast<btSoftBodyWorldInfo*>(scene->SoftBodyWorldInfoExternal), vertices, (int*)indices, numIndices / 3);

	delete vertices;
	delete indices;

	delete vertexBuffer;
    delete indexBuffer;
	delete meshPtr;

	scene->addSoftBodyProvider(this);	

	return softBody;
}

void BulletOgreSoftBodyProvider::destroySoftBodyImpl(BulletScene^ scene)
{
	assert(softBody != 0);

	scene->removeSoftBodyProvider(this);

	SceneManager^ sceneManager = ogreScene->SceneManager;
	sceneManager->getRootSceneNode()->removeChild(node);
	sceneManager->destroyEntity(entity);
	sceneManager->destroySceneNode(node);

	delete softBody;
	softBody = 0;
}

SimElementDefinition^ BulletOgreSoftBodyProvider::saveToDefinition()
{
	BulletOgreSoftBodyProviderDefinition^ def = gcnew BulletOgreSoftBodyProviderDefinition(this->Name);
	def->MeshName = meshName;

	return def;
}

void BulletOgreSoftBodyProvider::updateOtherSubsystems()
{
	btSoftBody::tNodeArray& btNodes = softBody->m_nodes;

	//Gather data from ogre mesh
	MeshPtr^ meshPtr = entity->getMesh();

    SubMesh^ subMesh = meshPtr->Value->getSubMesh(0);
    VertexData^ vertexData = subMesh->vertexData;
    IndexData^ indexData = subMesh->indexData;
    if (subMesh->UseSharedVertices)
    {
        vertexData = meshPtr->Value->SharedVertexData;
    }

    VertexDeclaration^ vertexDeclaration = vertexData->vertexDeclaration;
	VertexElement^ positionElement = vertexDeclaration->findElementBySemantic(VertexElementSemantic::VES_POSITION);

    VertexBufferBinding^ vertexBinding = vertexData->vertexBufferBinding;
    HardwareVertexBufferSharedPtr^ vertexBuffer = vertexBinding->getBuffer(positionElement->getSource());
    unsigned int vertexSize = vertexBuffer->Value->getVertexSize();
    
    unsigned int numVertices = vertexBuffer->Value->getNumVertices();
    unsigned int positionOffset = positionElement->getOffset();

    // Get vertex data
	int index = 0;
	unsigned char* vertexBufferData = (unsigned char*)vertexBuffer->Value->lock(HardwareBuffer::LockOptions::HBL_NORMAL);
    float* elemStart;
    for (unsigned int i = 0; i < numVertices; ++i)
    {
        positionElement->baseVertexPointerToElement(vertexBufferData, &elemStart);
        *elemStart++ = btNodes[index].m_x.x();
        *elemStart++ = btNodes[index].m_x.y();
        *elemStart++ = btNodes[index].m_x.z();
        vertexBufferData += vertexSize;
		index++;
    }
    vertexBuffer->Value->unlock();

	//Cleanup
	delete vertexBuffer;
	delete meshPtr;
}

}