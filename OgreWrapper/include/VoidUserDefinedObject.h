#pragma once

#include "OgreUserDefinedObject.h"

namespace Engine
{

namespace Rendering
{

enum ObjectType
{
	CAMERA_GCROOT,
	RENDER_ENTITY_GCROOT,
	LIGHT_GCROOT
};

class VoidUserDefinedObject : public Ogre::UserDefinedObject
{
public:
	void* object;
	ObjectType type;

	VoidUserDefinedObject(ObjectType type, void* object)
		:type(type), object(object)
	{
	}

	virtual ~VoidUserDefinedObject(void)
	{
	}
};

}

}