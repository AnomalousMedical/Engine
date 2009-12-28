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
meshName(def->MeshName),
groupName(def->GroupName),
mDupVertices(0),
mNewIndexes(0)
{
}

BulletOgreSoftBodyProvider::~BulletOgreSoftBodyProvider(void)
{
	if(mDupVertices != 0)
	{
		delete[] mDupVertices;
		mDupVertices = 0;
		delete[] mNewIndexes;
		mNewIndexes = 0;
	}
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

	//Clone the specified mesh
	OgreWrapper::MeshManager^ meshManager = OgreWrapper::MeshManager::getInstance();
	MeshPtr^ originalMesh = meshManager->load(meshName, groupName, HardwareBuffer::Usage::HBU_DYNAMIC, HardwareBuffer::Usage::HBU_STATIC, true, true);
	MeshPtr^ meshPtr = originalMesh->Value->clone(meshName + "_" + this->Name + "_SoftBody", "SoftBodyMeshes");
	meshPtr->Value->removeAllAnimations();
	meshPtr->Value->setSkeletonName("");

	//Create the ogre stuff
	SceneManager^ sceneManager = ogreScene->SceneManager;
	node = sceneManager->createSceneNode(this->Name + "__SoftBodyNode");
	entity = ogreScene->SceneManager->createEntity(this->Name, meshPtr->Value->getName());
	node->attachObject(entity);
	sceneManager->getRootSceneNode()->addChild(node);

	delete originalMesh;

	//Gather data from ogre mesh
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

	mDupVertices = new int[numVertices];
	mNewIndexes = new int[numVertices];
	mDupVerticesCount = 0;

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

	//Search for duplicate vertices
	verticesPtr = (Vector3*)vertices;
	for(int i=0; i < numVertices; i++)
	{
		Vector3 v1 =  verticesPtr[i];
		mDupVertices[i] = -1;
		mNewIndexes[i] = i - mDupVerticesCount;
		for(int j=0; j < i; j++)
		{
			Vector3 v2 =  verticesPtr[j];
			if (v1 == v2) {
				mDupVertices[i] = j;
				mDupVerticesCount++;
				break;
			}
		}
	}
	//Logging::Log::Debug("Num duplicate vertices {0}", mDupVerticesCount);

	//Reassign duplicate vertices
	int newVertexCount = numVertices - mDupVerticesCount;
	//Logging::Log::Debug("new vertex count {0}", newVertexCount);
	float* sbVertices = new float[newVertexCount * 3];
	int j=0;
	for(int i=0; i < numVertices; i++)
	{
		if (mDupVertices[i] == -1) {
			Vector3 v =  verticesPtr[i];
			sbVertices[j++] = v.x;
			sbVertices[j++] = v.y;
			sbVertices[j++] = v.z;
		}
	}

	//Reassign indices
	int* sbIndexes = new int[numIndices];
	for(int i=0; i < numIndices; i++)
	{
		sbIndexes[i] = getBulletIndex(indices[i]);
	}
	int sbTriangles = numIndices / 3;

	softBody = createFromTriMesh(static_cast<btSoftBodyWorldInfo*>(scene->SoftBodyWorldInfoExternal), sbVertices, sbIndexes, sbTriangles);

	delete[] sbIndexes;
	delete[] sbVertices;
	delete[] vertices;
	delete[] indices;

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
	def->GroupName = groupName;
	def->RenderQueue = entity->getRenderQueueGroup();
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

	//Get first vertex as origin
	Vector3 origin(btNodes[0].m_x.x(), btNodes[0].m_x.y(), btNodes[0].m_x.z());
	node->setPosition(origin);
	this->UpdatingPosition = true;
	Quaternion ident = Quaternion::Identity;
	this->updatePosition(origin, ident);
	this->UpdatingPosition = false;

    // Get vertex data
	int index = 0;
	int ogreIndex = 0;
	unsigned char* vertexBufferData = (unsigned char*)vertexBuffer->Value->lock(HardwareBuffer::LockOptions::HBL_NORMAL);
    float* elemStart;
    for (unsigned int i = 0; i < numVertices; ++i)
    {
		index = getBulletIndex(ogreIndex);
        positionElement->baseVertexPointerToElement(vertexBufferData, &elemStart);
        *elemStart++ = btNodes[index].m_x.x() - origin.x;
        *elemStart++ = btNodes[index].m_x.y() - origin.y;
        *elemStart++ = btNodes[index].m_x.z() - origin.z;
        vertexBufferData += vertexSize;
		ogreIndex++;
    }
    vertexBuffer->Value->unlock();

	//Cleanup
	delete vertexBuffer;
	delete meshPtr;
}

int BulletOgreSoftBodyProvider::getBulletIndex(int idx) 
{
	int idxDup = mDupVertices[idx];
	return mNewIndexes[idxDup == -1 ? idx : idxDup];
}

}