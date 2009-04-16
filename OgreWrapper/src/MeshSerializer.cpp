#include "StdAfx.h"
#include "..\include\MeshSerializer.h"

#include "OgreMeshSerializer.h"
#include "MarshalUtils.h"
#include "Mesh.h"

namespace Engine
{

namespace Rendering
{

MeshSerializer::MeshSerializer()
:meshSerializer( new Ogre::MeshSerializer() )
{

}

MeshSerializer::~MeshSerializer()
{
	
}

Ogre::MeshSerializer* MeshSerializer::getMeshSerializer()
{
	return meshSerializer.Get();
}

void MeshSerializer::exportMesh(Mesh^ mesh, System::String^ filename)
{
	meshSerializer->exportMesh(mesh->getOgreMesh(), MarshalUtils::convertString(filename));
}

void MeshSerializer::exportMesh(Mesh^ mesh, System::String^ filename, Endian endianMode)
{
	meshSerializer->exportMesh(mesh->getOgreMesh(), MarshalUtils::convertString(filename), static_cast<Ogre::Serializer::Endian>(endianMode));
}

}

}