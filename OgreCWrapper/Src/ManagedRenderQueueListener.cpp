#include "Stdafx.h"

typedef void (*EmptyRenderQueueEvent)();
typedef bool (*ByteStringBoolRenderQueueEvent)(Ogre::uint8 queueGroupId, String invocation);

class ManagedRenderQueueListener : public Ogre::RenderQueueListener
{
private:
        EmptyRenderQueueEvent preRenderQueuesCallback;
        EmptyRenderQueueEvent postRenderQueuesCallback;
        ByteStringBoolRenderQueueEvent renderQueueStartedCallback;
        ByteStringBoolRenderQueueEvent renderQueueEndedCallback;

public:
	ManagedRenderQueueListener(EmptyRenderQueueEvent preRenderQueuesCallback, EmptyRenderQueueEvent postRenderQueuesCallback, ByteStringBoolRenderQueueEvent renderQueueStartedCallback, ByteStringBoolRenderQueueEvent renderQueueEndedCallback)
		:preRenderQueuesCallback(preRenderQueuesCallback),
		postRenderQueuesCallback(postRenderQueuesCallback),
		renderQueueStartedCallback(renderQueueStartedCallback),
		renderQueueEndedCallback(renderQueueEndedCallback)
	{

	}

	virtual ~ManagedRenderQueueListener()
	{

	}

	virtual void preRenderQueues()
	{
		preRenderQueuesCallback();
	}

	virtual void postRenderQueues()
	{
		postRenderQueuesCallback();
	}

	virtual void renderQueueStarted(Ogre::uint8 queueGroupId, const Ogre::String &invocation, bool &skipThisInvocation)
	{
		skipThisInvocation = renderQueueStartedCallback(queueGroupId, invocation.c_str());
	}

	virtual void renderQueueEnded(Ogre::uint8 queueGroupId, const Ogre::String &invocation, bool &repeatThisInvocation)
	{
		repeatThisInvocation = renderQueueEndedCallback(queueGroupId, invocation.c_str());
	}
};

extern "C" _AnomalousExport ManagedRenderQueueListener* NativeRenderQueue_Create(EmptyRenderQueueEvent preRender, EmptyRenderQueueEvent postRender, ByteStringBoolRenderQueueEvent renderStarted, ByteStringBoolRenderQueueEvent renderEnded)
{
	return new ManagedRenderQueueListener(preRender, postRender, renderStarted, renderEnded);
}

extern "C" _AnomalousExport void NativeRenderQueue_Delete(ManagedRenderQueueListener* nativeRenderQueueListener)
{
	delete nativeRenderQueueListener;
}

extern "C" _AnomalousExport void NativeRenderQueue_AddListener(Ogre::SceneManager* sceneManager, ManagedRenderQueueListener* nativeRenderQueueListener)
{
	sceneManager->addRenderQueueListener(nativeRenderQueueListener);
}

extern "C" _AnomalousExport void NativeRenderQueue_RemoveListener(Ogre::SceneManager* sceneManager, ManagedRenderQueueListener* nativeRenderQueueListener)
{
	sceneManager->removeRenderQueueListener(nativeRenderQueueListener);
}