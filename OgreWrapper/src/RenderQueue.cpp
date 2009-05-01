#include "StdAfx.h"
#include "..\include\RenderQueue.h"

#include "Renderable.h"
#include "OgreRenderQueue.h"

namespace OgreWrapper
{

RenderQueue::RenderQueue(Ogre::RenderQueue* renderQueue)
:renderQueue( renderQueue )
{

}

RenderQueue::~RenderQueue()
{
	renderQueue = 0;
}

Ogre::RenderQueue* RenderQueue::getRenderQueue()
{
	return renderQueue;
}

void RenderQueue::addRenderable(Renderable^ renderable, unsigned char groupID, unsigned short priority)
{
	return renderQueue->addRenderable(renderable->getRenderable(), groupID, priority);
}

void RenderQueue::addRenderable(Renderable^ renderable, unsigned char groupID)
{
	return renderQueue->addRenderable(renderable->getRenderable(), groupID);
}

void RenderQueue::addRenderable(Renderable^ renderable)
{
	return renderQueue->addRenderable(renderable->getRenderable());
}

unsigned char RenderQueue::getDefaultQueueGroup()
{
	return renderQueue->getDefaultQueueGroup();
}

void RenderQueue::setDefaultRenderablePriority(unsigned short priority)
{
	return renderQueue->setDefaultRenderablePriority(priority);
}

unsigned short RenderQueue::getDefaultRenderablePriority()
{
	return renderQueue->getDefaultRenderablePriority();
}

void RenderQueue::setDefaultQueueGroup(unsigned char grp)
{
	return renderQueue->setDefaultQueueGroup(grp);
}

void RenderQueue::setSplitPassesByLightingType(bool split)
{
	return renderQueue->setSplitPassesByLightingType(split);
}

void RenderQueue::setSplitNoShadowPasses(bool split)
{
	return renderQueue->setSplitNoShadowPasses(split);
}

void RenderQueue::setShadowCastersCannotBeReceivers(bool ind)
{
	return renderQueue->setShadowCastersCannotBeReceivers(ind);
}

}