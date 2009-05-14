#pragma once

namespace PhysXWrapper
{

value class PhysDebugLine;
value class PhysDebugPoint;
value class PhysDebugTriangle;

public ref class PhysDebugRenderable
{
private:
	const NxDebugRenderable* debugRenderable;

public:
	PhysDebugRenderable(void);

	virtual ~PhysDebugRenderable(void);

	NxU32 getNbPoints();

	const PhysDebugPoint* getPoints();

	NxU32 getNbLines();

	const PhysDebugLine* getLines();

	NxU32 getNbTriangles();

	const PhysDebugTriangle* getTriangles();

internal:
	void setDebugRenderable(const NxDebugRenderable* debugRenderable);
};

}