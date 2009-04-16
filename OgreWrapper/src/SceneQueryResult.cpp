#include "stdafx.h"
#include "SceneQueryResult.h"

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