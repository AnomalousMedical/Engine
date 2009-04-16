#include "stdafx.h"
#include "SceneQueryResult.h"

namespace OgreWrapper
{

SceneQueryResult::SceneQueryResult()
:movables(gcnew SceneQueryResultMovableList())
{

}

void SceneQueryResult::clear()
{
	movables->Clear();	
}

}