//Source
#include "stdafx.h"
#include "PhysShapeCollection.h"
#include "PhysShape.h"
#include "PhysBoxShape.h"
#include "PhysCapsuleShape.h"
#include "PhysConvexShape.h"
#include "PhysPlaneShape.h"
#include "PhysSphereShape.h"
#include "PhysTriangleMeshShape.h"

namespace Physics{

PhysShape^ PhysShapeCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	NxShape* nxShape = static_cast<NxShape*>(nativeObject);
	switch(nxShape->getType())
	{
		case NX_SHAPE_PLANE:
			return gcnew PhysPlaneShape((NxPlaneShape*)nxShape);
			break;
		case NX_SHAPE_SPHERE:
			return gcnew PhysSphereShape((NxSphereShape*)nxShape);
			break;
		case NX_SHAPE_BOX:
			return gcnew PhysBoxShape((NxBoxShape*)nxShape);
			break;
		case NX_SHAPE_CAPSULE:
			return gcnew PhysCapsuleShape((NxCapsuleShape*)nxShape);
			break;
		case NX_SHAPE_WHEEL:
			throw gcnew System::NotImplementedException("Shape classes for NX_SHAPE_WHEEL not implemented");
			break;
		case NX_SHAPE_CONVEX:
			return gcnew PhysConvexShape((NxConvexShape*)nxShape);
			break;
		case NX_SHAPE_MESH:
			return gcnew PhysTriangleMeshShape((NxTriangleMeshShape*)nxShape);
			break;
		case NX_SHAPE_HEIGHTFIELD:
			throw gcnew System::NotImplementedException("Shape classes for NX_SHAPE_HEIGHTFIELD not implemented");
			break;
		default:
			throw gcnew System::NotImplementedException("Somehow got unknown shape type.");
			break;
	}
}

PhysShape^ PhysShapeCollection::getObject(NxShape* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void PhysShapeCollection::destroyObject(NxShape* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}