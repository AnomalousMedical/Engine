#include "StdAfx.h"

typedef void(*RenderTargetEventDelegate)();
typedef void(*RenderTargetViewportEventDelegate)();

class ManagedRenderTargetListener : Ogre::RenderTargetListener
{
public:

	ManagedRenderTargetListener(RenderTargetEventDelegate preRenderTargetUpdateCb, RenderTargetEventDelegate postRenderTargetUpdateCb, RenderTargetViewportEventDelegate preViewportUpdateCb, RenderTargetViewportEventDelegate postViewportUpdateCb, RenderTargetViewportEventDelegate viewportAddedCb, RenderTargetViewportEventDelegate viewportRemovedCb)
		:preRenderTargetUpdateCb(preRenderTargetUpdateCb),
		postRenderTargetUpdateCb(postRenderTargetUpdateCb),
		preViewportUpdateCb(preViewportUpdateCb),
		postViewportUpdateCb(postViewportUpdateCb),
		viewportAddedCb(viewportAddedCb),
		viewportRemovedCb(viewportRemovedCb)
	{

	}

	virtual ~ManagedRenderTargetListener()
	{

	}

	virtual void preRenderTargetUpdate(const Ogre::RenderTargetEvent& evt)
	{
		preRenderTargetUpdateCb();
	}
 
	virtual void postRenderTargetUpdate(const Ogre::RenderTargetEvent& evt)
	{
		postRenderTargetUpdateCb();
	}

	virtual void preViewportUpdate(const Ogre::RenderTargetViewportEvent& evt)
	{
		preViewportUpdateCb();
	}

	virtual void postViewportUpdate(const Ogre::RenderTargetViewportEvent& evt)
	{
		postViewportUpdateCb();
	}
 
	virtual void viewportAdded(const Ogre::RenderTargetViewportEvent& evt)
	{
		viewportAddedCb();
	}

	virtual void viewportRemoved(const Ogre::RenderTargetViewportEvent& evt)
	{
		viewportRemovedCb();
	}

private:
	RenderTargetEventDelegate preRenderTargetUpdateCb;
	RenderTargetEventDelegate postRenderTargetUpdateCb;
	RenderTargetViewportEventDelegate preViewportUpdateCb;
	RenderTargetViewportEventDelegate postViewportUpdateCb;
	RenderTargetViewportEventDelegate viewportAddedCb;
	RenderTargetViewportEventDelegate viewportRemovedCb;
};

extern "C" _AnomalousExport ManagedRenderTargetListener* ManagedRenderTargetListener_Create(RenderTargetEventDelegate preRenderTargetUpdateCb, RenderTargetEventDelegate postRenderTargetUpdateCb, RenderTargetViewportEventDelegate preViewportUpdateCb, RenderTargetViewportEventDelegate postViewportUpdateCb, RenderTargetViewportEventDelegate viewportAddedCb, RenderTargetViewportEventDelegate viewportRemovedCb)
{
	return new ManagedRenderTargetListener(preRenderTargetUpdateCb, postRenderTargetUpdateCb, preViewportUpdateCb, postViewportUpdateCb, viewportAddedCb, viewportRemovedCb);
}

extern "C" _AnomalousExport void ManagedRenderTargetListener_Delete(ManagedRenderTargetListener *listener)
{
	delete listener;
}