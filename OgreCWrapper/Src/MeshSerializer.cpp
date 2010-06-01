#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::MeshSerializer* MeshSerializer_Create()
{
	return new Ogre::MeshSerializer();
}

extern "C" __declspec(dllexport) void MeshSerializer_Delete(Ogre::MeshSerializer* meshSerializer)
{
	delete meshSerializer;
}

extern "C" __declspec(dllexport) void MeshSerializer_exportMesh(Ogre::MeshSerializer* meshSerializer, Ogre::Mesh* mesh, String filename)
{
	meshSerializer->exportMesh(mesh, filename);
}

extern "C" __declspec(dllexport) void MeshSerializer_exportMeshEndian(Ogre::MeshSerializer* meshSerializer, Ogre::Mesh* mesh, String filename, Ogre::MeshSerializer::Endian endianMode)
{
	meshSerializer->exportMesh(mesh, filename, endianMode);
}