#pragma once

class NxTriangleMesh;

namespace Engine
{

namespace Physics
{

ref class PhysTriangleMeshDesc;

[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysTriangleMesh
{
internal:
	PhysTriangleMesh(NxTriangleMesh* triangleMesh);

	NxTriangleMesh* triangleMesh;

public:
	bool saveToDesc(PhysTriangleMeshDesc^ desc);

	unsigned int getReferenceCount();
};

}

}