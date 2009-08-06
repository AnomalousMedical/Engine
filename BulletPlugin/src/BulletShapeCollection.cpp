#include "StdAfx.h"
#include "..\include\BulletShapeCollection.h"

namespace BulletPlugin
{

BulletShapeCollection::BulletShapeCollection(btCollisionShape* collisionShape, String^ name)
:collisionShape(collisionShape),
name(name), 
sourceLocation(nullptr)
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