#include "stdafx.h"
#include "SceneQueryResult.h"

namespace Engine
{

namespace Rendering
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

}