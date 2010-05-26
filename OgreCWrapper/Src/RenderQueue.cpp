#include "Stdafx.h"

extern "C" __declspec(dllexport) byte RenderQueue_getDefaultQueueGroup(Ogre::RenderQueue* renderQueue)
{
	return renderQueue->getDefaultQueueGroup();
}

extern "C" __declspec(dllexport) void RenderQueue_setDefaultRenderablePriority(Ogre::RenderQueue* renderQueue, ushort priority)
{
	renderQueue->setDefaultRenderablePriority(priority);
}

extern "C" __declspec(dllexport) ushort RenderQueue_getDefaultRenderablePriority(Ogre::RenderQueue* renderQueue)
{
	return renderQueue->getDefaultRenderablePriority();
}

extern "C" __declspec(dllexport) void RenderQueue_setDefaultQueueGroup(Ogre::RenderQueue* renderQueue, byte grp)
{
	renderQueue->setDefaultQueueGroup(grp);
}

extern "C" __declspec(dllexport) void RenderQueue_setSplitPassesByLightingType(Ogre::RenderQueue* renderQueue, bool split)
{
	renderQueue->setSplitPassesByLightingType(split);
}

extern "C" __declspec(dllexport) void RenderQueue_setSplitNoShadowPasses(Ogre::RenderQueue* renderQueue, bool split)
{
	renderQueue->setSplitNoShadowPasses(split);
}

extern "C" __declspec(dllexport) void RenderQueue_setShadowCastersCannotBeReceivers(Ogre::RenderQueue* renderQueue, bool ind)
{
	renderQueue->setShadowCastersCannotBeReceivers(ind);
}