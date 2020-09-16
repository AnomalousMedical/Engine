#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::v1::MeshSerializer* MeshSerializer_Create()
{
	return new Ogre::v1::MeshSerializer();
}

extern "C" _AnomalousExport void MeshSerializer_Delete(Ogre::v1::MeshSerializer* meshSerializer)
{
	delete meshSerializer;
}

extern "C" _AnomalousExport void MeshSerializer_exportMesh(Ogre::v1::MeshSerializer* meshSerializer, Ogre::v1::Mesh* mesh, String filename)
{
	meshSerializer->exportMesh(mesh, filename);
}

extern "C" _AnomalousExport void MeshSerializer_exportMeshEndian(Ogre::v1::MeshSerializer* meshSerializer, Ogre::v1::Mesh* mesh, String filename, Ogre::v1::MeshSerializer::Endian endianMode)
{
	meshSerializer->exportMesh(mesh, filename, endianMode);
}