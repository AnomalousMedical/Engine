#include "StdAfx.h"

class ManagedEventListenerInstancer : public Rocket::Core::EventListenerInstancer
{
public:
	typedef Rocket::Core::EventListener* (*InstanceEventListenerCb)(String name);
	typedef void (*ReleaseCb)();

	ManagedEventListenerInstancer(InstanceEventListenerCb instanceEventListener, ReleaseCb release)
		:instanceEventListener(instanceEventListener),
		release(release)
	{

	}

	virtual ~ManagedEventListenerInstancer() {}

	/// Instance and event listener object
	/// @param value Value of the event	
	virtual Rocket::Core::EventListener* InstanceEventListener(const Rocket::Core::String& value)
	{
		return instanceEventListener(value.CString());
	}

	/// Releases this event listener instancer
	virtual void Release()
	{
		release();
	}

private:
	InstanceEventListenerCb instanceEventListener;
	ReleaseCb release;
};

extern "C" _AnomalousExport ManagedEventListenerInstancer* ManagedEventListenerInstancer_Create(ManagedEventListenerInstancer::InstanceEventListenerCb instanceEventListener, ManagedEventListenerInstancer::ReleaseCb release)
{
	return new ManagedEventListenerInstancer(instanceEventListener, release);
}

extern "C" _AnomalousExport void ManagedEventListenerInstancer_Delete(ManagedEventListenerInstancer* eventListenerInstancer)
{
	delete eventListenerInstancer;
}