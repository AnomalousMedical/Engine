#include "StdAfx.h"

typedef void (*EventOccuredCallback)(HANDLE_FIRST_ARG);

enum KnownRenderSystemEvents
{
	RenderSystemCapabilitiesCreated,
	DeviceLost,
	DeviceRestored,
	Unknown
};

class ManagedRenderSystemListener : Ogre::RenderSystem::Listener
{
public:

	ManagedRenderSystemListener(EventOccuredCallback eventOccuredCb HANDLE_ARG)
		:eventOccuredCb(eventOccuredCb),
		lastEventName("")
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~ManagedRenderSystemListener()
	{

	}

	virtual void eventOccurred(const Ogre::String &eventName, const Ogre::NameValuePairList *parameters = 0 )
	{
		lastEventName = eventName;
		eventOccuredCb(PASS_HANDLE);
	}

	//This function will change an event string into an enum, 
	//this way we can avoid marshaling the event string if the event type is already known.
	KnownRenderSystemEvents getEventType()
	{
		if(lastEventName == "RenderSystemCapabilitiesCreated")
		{
			return RenderSystemCapabilitiesCreated;
		}
		else if(lastEventName == "DeviceLost")
		{
			return DeviceLost;
		}
		else if(lastEventName == "DeviceRestored")
		{
			return DeviceRestored;
		}
		else
		{
			return Unknown;
		}
	}

private:
	EventOccuredCallback eventOccuredCb;
	Ogre::String lastEventName;
	HANDLE_INSTANCE
};

extern "C" _AnomalousExport ManagedRenderSystemListener* ManagedRenderSystemListener_Create(EventOccuredCallback eventOccuredCb HANDLE_ARG)
{
	return new ManagedRenderSystemListener(eventOccuredCb PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void ManagedRenderSystemListener_Delete(ManagedRenderSystemListener *listener)
{
	delete listener;
}

extern "C" _AnomalousExport KnownRenderSystemEvents ManagedRenderSystemListener_getEventType(ManagedRenderSystemListener *listener)
{
	return listener->getEventType();
}