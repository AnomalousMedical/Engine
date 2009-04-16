#pragma once

namespace OgreWrapper{

ref class MovableObject;

public ref class RaySceneQueryResultEntry
{
public:
	RaySceneQueryResultEntry(float distance, MovableObject^ obj);

	float distance;

	MovableObject^ movable;
};

}