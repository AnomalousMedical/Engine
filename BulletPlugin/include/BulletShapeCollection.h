#pragma once

using namespace System;
using namespace Engine;

namespace BulletPlugin
{

ref class BulletShapeCollection
{
private:
	btCollisionShape* collisionShape;
	String^ name;
	ShapeLocation^ sourceLocation;

public:
	BulletShapeCollection(btCollisionShape* collisionShape, String^ name);

	virtual ~BulletShapeCollection(void);

	property btCollisionShape* CollisionShape
	{
		btCollisionShape* get()
		{
			return collisionShape;
		}
	}

	property int Count
	{
		virtual int get()
		{
			return 1;
		}
	}

	property ShapeLocation^ SourceLocation
	{
		ShapeLocation^ get()
		{
			return sourceLocation;
		}
		void set(ShapeLocation^ value)
		{
			sourceLocation = value;
		}
	}

	property String^ Name
	{
		String^ get()
		{
			return name;
		}
	}
};

}