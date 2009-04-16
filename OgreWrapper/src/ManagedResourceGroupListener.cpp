#include "StdAfx.h"
#include "..\include\ManagedResourceGroupListener.h"
#include "ResourceGroupListener.h"

namespace Engine{

namespace Rendering{

ManagedResourceGroupListener::ManagedResourceGroupListener(void)
:listeners(gcnew ResourceGroupListenerList())
{

}

void ManagedResourceGroupListener::addListener(ResourceGroupListener^ listener)
{
	listeners->AddLast(listener);
}

void ManagedResourceGroupListener::removeListener(ResourceGroupListener^ listener)
{
	listeners->Remove(listener);
}

void ManagedResourceGroupListener::resourceGroupScriptingStarted(System::String^ groupName, int scriptCount)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceGroupScriptingStarted(groupName, scriptCount);
	}
}

void ManagedResourceGroupListener::scriptParseStarted(System::String^ scriptName, bool% skipThisScript)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->scriptParseStarted(scriptName, skipThisScript);
	}
}

void ManagedResourceGroupListener::scriptParseEnded(System::String^ scriptName, bool skipped)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->scriptParseEnded(scriptName, skipped);
	}
}

void ManagedResourceGroupListener::resourceGroupScriptingEnded(System::String^ groupName)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceGroupScriptingEnded(groupName);
	}
}

void ManagedResourceGroupListener::resourceGroupPrepareStarted(System::String^ groupName, int resourceCount)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceGroupPrepareStarted(groupName, resourceCount);
	}
}

void ManagedResourceGroupListener::resourcePrepareStarted(OgreResource^ resource)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourcePrepareStarted(resource);
	}
}

void ManagedResourceGroupListener::resourcePrepareEnded()
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourcePrepareEnded();
	}
}

void ManagedResourceGroupListener::worldGeometryPrepareStageStarted(System::String^ description)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->worldGeometryPrepareStageStarted(description);
	}
}

void ManagedResourceGroupListener::worldGeometryPrepareStageEnded()
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->worldGeometryPrepareStageEnded();
	}
}

void ManagedResourceGroupListener::resourceGroupPrepareEnded(System::String^ groupName)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceGroupPrepareEnded(groupName);
	}
}

void ManagedResourceGroupListener::resourceGroupLoadStarted(System::String^ groupName, int resourceCount)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceGroupLoadStarted(groupName, resourceCount);
	}
}

void ManagedResourceGroupListener::resourceLoadStarted(OgreResource^ resource)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceLoadStarted(resource);
	}
}

void ManagedResourceGroupListener::resourceLoadEnded()
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceLoadEnded();
	}
}

void ManagedResourceGroupListener::worldGeometryStageStarted(System::String^ description)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->worldGeometryStageStarted(description);
	}
}

void ManagedResourceGroupListener::worldGeometryStageEnded()
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->worldGeometryStageEnded();
	}
}

void ManagedResourceGroupListener::resourceGroupLoadEnded(System::String^ groupName)
{
	for each(ResourceGroupListener^ listener in listeners)
	{
		listener->resourceGroupLoadEnded(groupName);
	}
}

}

}