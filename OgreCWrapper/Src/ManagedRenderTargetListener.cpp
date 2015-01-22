#include "StdAfx.h"

class ManagedRenderTargetListener : Ogre::RenderTargetListener
{
public:

	ManagedRenderTargetListener(NativeAction preRenderTargetUpdateCb, NativeAction postRenderTargetUpdateCb, NativeAction preViewportUpdateCb, NativeAction postViewportUpdateCb, NativeAction viewportAddedCb, NativeAction viewportRemovedCb HANDLE_ARG)
		:preRenderTargetUpdateCb(preRenderTargetUpdateCb),
		postRenderTargetUpdateCb(postRenderTargetUpdateCb),
		preViewportUpdateCb(preViewportUpdateCb),
		postViewportUpdateCb(postViewportUpdateCb),
		viewportAddedCb(viewportAddedCb),
		viewportRemovedCb(viewportRemovedCb)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedRenderTargetListener()
	{

	}

	virtual void preRenderTargetUpdate(const Ogre::RenderTargetEvent& evt)
	{
		preRenderTargetUpdateCb(PASS_HANDLE);
	}
 
	virtual void postRenderTargetUpdate(const Ogre::RenderTargetEvent& evt)
	{
		postRenderTargetUpdateCb(PASS_HANDLE);
	}

	virtual void preViewportUpdate(const Ogre::RenderTargetViewportEvent& evt)
	{
		preViewportUpdateCb(PASS_HANDLE);
	}

	virtual void postViewportUpdate(const Ogre::RenderTargetViewportEvent& evt)
	{
		postViewportUpdateCb(PASS_HANDLE);
	}
 
	virtual void viewportAdded(const Ogre::RenderTargetViewportEvent& evt)
	{
		viewportAddedCb(PASS_HANDLE);
	}

	virtual void viewportRemoved(const Ogre::RenderTargetViewportEvent& evt)
	{
		viewportRemovedCb(PASS_HANDLE);
	}

private:
	NativeAction preRenderTargetUpdateCb;
	NativeAction postRenderTargetUpdateCb;
	NativeAction preViewportUpdateCb;
	NativeAction postViewportUpdateCb;
	NativeAction viewportAddedCb;
	NativeAction viewportRemovedCb;
	HANDLE_INSTANCE
};

extern "C" _AnomalousExport ManagedRenderTargetListener* ManagedRenderTargetListener_Create(NativeAction preRenderTargetUpdateCb, NativeAction postRenderTargetUpdateCb, NativeAction preViewportUpdateCb, NativeAction postViewportUpdateCb, NativeAction viewportAddedCb, NativeAction viewportRemovedCb HANDLE_ARG)
{
	return new ManagedRenderTargetListener(preRenderTargetUpdateCb, postRenderTargetUpdateCb, preViewportUpdateCb, postViewportUpdateCb, viewportAddedCb, viewportRemovedCb PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedRenderTargetListener_Delete(ManagedRenderTargetListener *listener)
{
	delete listener;
}