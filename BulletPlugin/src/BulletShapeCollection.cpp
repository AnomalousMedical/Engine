#include "StdAfx.h"
#include "..\include\BulletShapeCollection.h"

namespace BulletPlugin
{

BulletShapeCollection::BulletShapeCollection(btCollisionShape* collisionShape, String^ name)
:ShapeCollection(name),
collisionShape(collisionShape)
{
}

BulletShapeCollection::~BulletShapeCollection(void)
{
	if(collisionShape != 0)
	{
		delete collisionShape;
		collisionShape = 0;
	}
}

}