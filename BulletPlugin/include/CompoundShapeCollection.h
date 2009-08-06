#pragma once

#include "BulletShapeCollection.h"

namespace BulletPlugin
{

ref class CompoundShapeCollection : public BulletShapeCollection
{
private:
	btCompoundShape* compound;

public:
	CompoundShapeCollection(btCompoundShape* compound, String^ name);

	virtual ~CompoundShapeCollection(void);

	property int Count
	{
		virtual int get() override
		{
			return compound->getNumChildShapes();
		}
	}
};

}