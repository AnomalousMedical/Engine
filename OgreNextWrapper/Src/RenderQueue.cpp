#include "Stdafx.h"

extern "C" _AnomalousExport byte RenderQueue_getDefaultQueueGroup(Ogre::RenderQueue* renderQueue)
{
	return renderQueue->getDefaultQueueGroup();
}

extern "C" _AnomalousExport void RenderQueue_setDefaultRenderablePriority(Ogre::RenderQueue* renderQueue, ushort priority)
{
	renderQueue->setDefaultRenderablePriority(priority);
}

extern "C" _AnomalousExport ushort RenderQueue_getDefaultRenderablePriority(Ogre::RenderQueue* renderQueue)
{
	return renderQueue->getDefaultRenderablePriority();
}

extern "C" _AnomalousExport void RenderQueue_setDefaultQueueGroup(Ogre::RenderQueue* renderQueue, byte grp)
{
	renderQueue->setDefaultQueueGroup(grp);
}

extern "C" _AnomalousExport void RenderQueue_setSplitPassesByLightingType(Ogre::RenderQueue* renderQueue, bool split)
{
	renderQueue->setSplitPassesByLightingType(split);
}

extern "C" _AnomalousExport void RenderQueue_setSplitNoShadowPasses(Ogre::RenderQueue* renderQueue, bool split)
{
	renderQueue->setSplitNoShadowPasses(split);
}

extern "C" _AnomalousExport void RenderQueue_setShadowCastersCannotBeReceivers(Ogre::RenderQueue* renderQueue, bool ind)
{
	renderQueue->setShadowCastersCannotBeReceivers(ind);
}