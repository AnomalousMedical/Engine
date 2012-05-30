#include "StdAfx.h"

typedef void (*EventOccuredCallback)();

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

	ManagedRenderSystemListener(EventOccuredCallback eventOccuredCb)
		:eventOccuredCb(eventOccuredCb),
		lastEventName("")
	{

	}

	virtual ~ManagedRenderSystemListener()
	{

	}

	virtual void eventOccurred(const Ogre::String &eventName, const Ogre::NameValuePairList *parameters = 0 )
	{
		lastEventName = eventName;
		eventOccuredCb();
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
};

extern "C" _AnomalousExport ManagedRenderSystemListener* ManagedRenderSystemListener_Create(EventOccuredCallback eventOccuredCb)
{
	return new ManagedRenderSystemListener(eventOccuredCb);
}

extern "C" _AnomalousExport void ManagedRenderSystemListener_Delete(ManagedRenderSystemListener *listener)
{
	delete listener;
}

extern "C" _AnomalousExport KnownRenderSystemEvents ManagedRenderSystemListener_getEventType(ManagedRenderSystemListener *listener)
{
	return listener->getEventType();
}