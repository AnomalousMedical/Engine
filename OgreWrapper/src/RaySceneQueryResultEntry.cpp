#include "StdAfx.h"
#include "..\include\RaySceneQueryResultEntry.h"
#include "MovableObject.h"

namespace Engine {

namespace Rendering{

RaySceneQueryResultEntry::RaySceneQueryResultEntry(float distance, MovableObject^ obj)
:distance(distance),
movable(obj)
{
}

}

}