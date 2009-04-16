#include "StdAfx.h"
#include "..\include\RaySceneQueryResultEntry.h"
#include "MovableObject.h"

namespace OgreWrapper{

RaySceneQueryResultEntry::RaySceneQueryResultEntry(float distance, MovableObject^ obj)
:distance(distance),
movable(obj)
{
}

}