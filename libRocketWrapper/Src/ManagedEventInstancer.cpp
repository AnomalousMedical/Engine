#include "StdAfx.h"

class ManagedEventInstancer : public Rocket::Core::EventInstancer
{
public:
	typedef Rocket::Core::Event* (*InstanceEventCb)(Rocket::Core::Element* target, String name, const Rocket::Core::Dictionary* parameters, bool interuptable);
	typedef void (*ReleaseEventCb)(Rocket::Core::Event* event);
	typedef void (*ReleaseCb)();

	ManagedEventInstancer(InstanceEventCb instanceEvent, ReleaseEventCb releaseEvent, ReleaseCb release)
		:instanceEvent(instanceEvent),
		releaseEvent(releaseEvent),
		release(release)
	{

	}

	virtual ~ManagedEventInstancer()
	{

	}

	/// Instance an event
	virtual Rocket::Core::Event* InstanceEvent(Rocket::Core::Element* target, const Rocket::Core::String& name, const Rocket::Core::Dictionary& parameters, bool interuptable)
	{
		return instanceEvent(target, name.CString(), &parameters, interuptable);
	}

	/// Releases an event instanced by this instancer.
	/// @param[in] event The event to release.
	virtual void ReleaseEvent(Rocket::Core::Event* event)
	{
		releaseEvent(event);
	}

	/// Release this instancer
	virtual void Release()
	{
		release();
	}

private:
	InstanceEventCb instanceEvent;
	ReleaseEventCb releaseEvent;
	ReleaseCb release;
};

extern "C" _AnomalousExport ManagedEventInstancer* ManagedEventInstancer_Create(ManagedEventInstancer::InstanceEventCb instanceEvent, ManagedEventInstancer::ReleaseEventCb releaseEvent, ManagedEventInstancer::ReleaseCb release)
{
	return new ManagedEventInstancer(instanceEvent, releaseEvent, release);
}

extern "C" _AnomalousExport void ManagedEventInstancer_Delete(ManagedEventInstancer* eventInstancer)
{
	delete eventInstancer;
}