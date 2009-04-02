#include "StdAfx.h"
#include "..\include\PhysSoftBodyMesh.h"
#include "PhysSoftBodyMeshDesc.h"
#include "AutoPtr.h"

namespace PhysXWrapper
{

PhysSoftBodyMesh::PhysSoftBodyMesh(System::String^ name, NxSoftBodyMesh* softMesh)
:softMesh(softMesh), name(name)
{
	System::IntPtr key(softMesh);
	meshDictionary->Add(key, this);
}

PhysSoftBodyMesh::~PhysSoftBodyMesh()
{
	meshDictionary->Remove(System::IntPtr(softMesh));
	softMesh = 0;
}

bool PhysSoftBodyMesh::saveToDesc(PhysSoftBodyMeshDesc^ desc)
{
	desc->Name = name;
	return softMesh->saveToDesc(*desc->meshDesc.Get());
}

unsigned int PhysSoftBodyMesh::getReferenceCount()
{
	return softMesh->getReferenceCount();
}

PhysSoftBodyMesh^ PhysSoftBodyMesh::getMeshObject(NxSoftBodyMesh* softMesh)
{
	return meshDictionary[System::IntPtr(softMesh)];
}

}