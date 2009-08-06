#include "StdAfx.h"
#include "..\include\CompoundShapeCollection.h"

namespace BulletPlugin
{

CompoundShapeCollection::CompoundShapeCollection(btCompoundShape* compound, String^ name)
:BulletShapeCollection(compound, name),
compound(compound)
{

}

CompoundShapeCollection::~CompoundShapeCollection(void)
{
	int numShapes = compound->getNumChildShapes();
	for(int i = 0; i < numShapes; ++i)
	{
		btCollisionShape* shape = compound->getChildShape(i);
		delete shape;
	}
}

}