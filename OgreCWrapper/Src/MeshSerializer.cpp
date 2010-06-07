#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::MeshSerializer* MeshSerializer_Create()
{
	return new Ogre::MeshSerializer();
}

extern "C" _AnomalousExport void MeshSerializer_Delete(Ogre::MeshSerializer* meshSerializer)
{
	delete meshSerializer;
}

extern "C" _AnomalousExport void MeshSerializer_exportMesh(Ogre::MeshSerializer* meshSerializer, Ogre::Mesh* mesh, String filename)
{
	meshSerializer->exportMesh(mesh, filename);
}

extern "C" _AnomalousExport void MeshSerializer_exportMeshEndian(Ogre::MeshSerializer* meshSerializer, Ogre::Mesh* mesh, String filename, Ogre::MeshSerializer::Endian endianMode)
{
	meshSerializer->exportMesh(mesh, filename, endianMode);
}