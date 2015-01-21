#pragma once

class NativeFrameListener : public Ogre::FrameListener
{
private:
	NativeAction_Float_Float_NoHandle frameStartedCallback;
	NativeAction_Float_Float_NoHandle frameRenderingQueuedCallback;
	NativeAction_Float_Float_NoHandle frameEndedCallback;

public:
	NativeFrameListener(NativeAction_Float_Float_NoHandle frameStartedCallback, NativeAction_Float_Float_NoHandle frameRenderingQueuedCallback, NativeAction_Float_Float_NoHandle frameEndedCallback);

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
