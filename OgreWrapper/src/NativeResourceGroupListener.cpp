#include "StdAfx.h"
#include "..\include\NativeResourceGroupListener.h"
#include "ManagedResourceGroupListener.h"
#include "ResourceGroupListener.h"
#include "MarshalUtils.h"

namespace OgreWrapper{

NativeResourceGroupListener::NativeResourceGroupListener(void)
:managedListener(gcnew ManagedResourceGroupListener())
{
}

NativeResourceGroupListener::~NativeResourceGroupListener(void)
{
}

void NativeResourceGroupListener::addListener(gcroot<OgreWrapper::ResourceGroupListener^> listener)
{
	managedListener->addListener(listener);
}

void NativeResourceGroupListener::removeListener(gcroot<OgreWrapper::ResourceGroupListener^> listener)
{
	managedListener->removeListener(listener);
}

void NativeResourceGroupListener::resourceGroupScriptingStarted(const Ogre::String& groupName, size_t scriptCount)
{
	managedListener->resourceGroupScriptingStarted(MarshalUtils::convertString(groupName), scriptCount);
}

void NativeResourceGroupListener::scriptParseStarted(const Ogre::String& scriptName, bool& skipThisScript)
{
	managedListener->scriptParseStarted(MarshalUtils::convertString(scriptName), skipThisScript);
}

void NativeResourceGroupListener::scriptParseEnded(const Ogre::String& scriptName, bool skipped)
{
	managedListener->scriptParseEnded(MarshalUtils::convertString(scriptName), skipped);
}

void NativeResourceGroupListener::resourceGroupScriptingEnded(const Ogre::String& groupName)
{
	managedListener->resourceGroupScriptingEnded(MarshalUtils::convertString(groupName));
}

void NativeResourceGroupListener::resourceGroupPrepareStarted(const Ogre::String& groupName, size_t resourceCount)
{
	managedListener->resourceGroupPrepareStarted(MarshalUtils::convertString(groupName), resourceCount);
}

void NativeResourceGroupListener::resourcePrepareStarted(const Ogre::ResourcePtr& resource)
{
	//managedListener->resourcePrepareStarted();
}

void NativeResourceGroupListener::resourcePrepareEnded()
{
	managedListener->resourcePrepareEnded();
}

void NativeResourceGroupListener::worldGeometryPrepareStageStarted(const Ogre::String& description)
{
	managedListener->worldGeometryPrepareStageStarted(MarshalUtils::convertString(description));
}

void NativeResourceGroupListener::worldGeometryPrepareStageEnded()
{
	managedListener->worldGeometryPrepareStageEnded();
}

void NativeResourceGroupListener::resourceGroupPrepareEnded(const Ogre::String& groupName)
{
	managedListener->resourceGroupPrepareEnded(MarshalUtils::convertString(groupName));
}

void NativeResourceGroupListener::resourceGroupLoadStarted(const Ogre::String& groupName, size_t resourceCount)
{
	managedListener->resourceGroupLoadStarted(MarshalUtils::convertString(groupName), resourceCount);
}

void NativeResourceGroupListener::resourceLoadStarted(const Ogre::ResourcePtr& resource)
{
	//managedListener->resourceLoadStarted();
}

void NativeResourceGroupListener::resourceLoadEnded()
{
	managedListener->resourceLoadEnded();
}

void NativeResourceGroupListener::worldGeometryStageStarted(const Ogre::String& description)
{
	managedListener->worldGeometryStageStarted(MarshalUtils::convertString(description));
}

void NativeResourceGroupListener::worldGeometryStageEnded()
{
	managedListener->worldGeometryStageEnded();
}

void NativeResourceGroupListener::resourceGroupLoadEnded(const Ogre::String& groupName)
{
	managedListener->resourceGroupLoadEnded(MarshalUtils::convertString(groupName));
}

}