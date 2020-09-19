#pragma once

typedef void(*FrameEventCallback)(float arg0, float arg1);

class NativeFrameListener : public Ogre::FrameListener
{
private:
	FrameEventCallback frameStartedCallback;
	FrameEventCallback frameRenderingQueuedCallback;
	FrameEventCallback frameEndedCallback;

public:
	NativeFrameListener(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback);

	virtual ~NativeFrameListener(void);

	virtual bool frameStarted(const Ogre::FrameEvent& evt)
	{
		frameStartedCallback(evt.timeSinceLastEvent, evt.timeSinceLastFrame);
		return true; 
	}

	virtual bool frameRenderingQueued(const Ogre::FrameEvent& evt)
	{ 
		frameRenderingQueuedCallback(evt.timeSinceLastEvent, evt.timeSinceLastFrame);
		return true; 
	}

	virtual bool frameEnded(const Ogre::FrameEvent& evt)
	{ 
		frameEndedCallback(evt.timeSinceLastEvent, evt.timeSinceLastFrame);
		return true;  
	}
};
