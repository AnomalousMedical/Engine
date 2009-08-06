#pragma once

using namespace System;
using namespace Engine;

namespace BulletPlugin
{

ref class BulletShapeCollection : public ShapeCollection
{
private:
	btCollisionShape* collisionShape;
	String^ name;

public:
	BulletShapeCollection(btCollisionShape* collisionShape, String^ name);

	virtual ~BulletShapeCollection(void);

	property int Count
	{
		virtual int get() override
		{
			return 1;
		}
	}
};

}