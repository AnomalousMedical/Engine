#include "Stdafx.h"

typedef void (*EmptyRenderQueueEvent)(HANDLE_FIRST_ARG);
typedef bool(*ByteStringBoolRenderQueueEvent)(Ogre::uint8 queueGroupId, String invocation HANDLE_ARG);

class ManagedRenderQueueListener : public Ogre::RenderQueueListener
{
private:
        EmptyRenderQueueEvent preRenderQueuesCallback;
        EmptyRenderQueueEvent postRenderQueuesCallback;
        ByteStringBoolRenderQueueEvent renderQueueStartedCallback;
        ByteStringBoolRenderQueueEvent renderQueueEndedCallback;
		HANDLE_INSTANCE

public:
	ManagedRenderQueueListener(EmptyRenderQueueEvent preRenderQueuesCallback, EmptyRenderQueueEvent postRenderQueuesCallback, ByteStringBoolRenderQueueEvent renderQueueStartedCallback, ByteStringBoolRenderQueueEvent renderQueueEndedCallback HANDLE_ARG)
		:preRenderQueuesCallback(preRenderQueuesCallback),
		postRenderQueuesCallback(postRenderQueuesCallback),
		renderQueueStartedCallback(renderQueueStartedCallback),
		renderQueueEndedCallback(renderQueueEndedCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedRenderQueueListener()
	{

	}

	virtual void preRenderQueues()
	{
		preRenderQueuesCallback(PASS_HANDLE);
	}

	virtual void postRenderQueues()
	{
		postRenderQueuesCallback(PASS_HANDLE);
	}

	virtual void renderQueueStarted(Ogre::uint8 queueGroupId, const Ogre::String &invocation, bool &skipThisInvocation)
	{
		skipThisInvocation = renderQueueStartedCallback(queueGroupId, invocation.c_str() PASS_HANDLE_ARG);
	}

	virtual void renderQueueEnded(Ogre::uint8 queueGroupId, const Ogre::String &invocation, bool &repeatThisInvocation)
	{
		repeatThisInvocation = renderQueueEndedCallback(queueGroupId, invocation.c_str() PASS_HANDLE_ARG);
	}
};

extern "C" _AnomalousExport ManagedRenderQueueListener* NativeRenderQueue_Create(EmptyRenderQueueEvent preRender, EmptyRenderQueueEvent postRender, ByteStringBoolRenderQueueEvent renderStarted, ByteStringBoolRenderQueueEvent renderEnded HANDLE_ARG)
{
	return new ManagedRenderQueueListener(preRender, postRender, renderStarted, renderEnded PASS_HANDLE_ARG);
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