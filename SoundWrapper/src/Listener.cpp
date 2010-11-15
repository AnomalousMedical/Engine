#include "StdAfx.h"
#include "Listener.h"

namespace SoundWrapper
{

Listener::Listener(void)
{
}

Listener::~Listener(void)
{
}

}

//CWrapper
using namespace SoundWrapper;

extern "C" _AnomalousExport float Listener_getGain(Listener* listener)
{
	return listener->getGain();
}

extern "C" _AnomalousExport void Listener_setGain(Listener* listener, float value)
{
	listener->setGain(value);
}

extern "C" _AnomalousExport Vector3 Listener_getPosition(Listener* listener)
{
	return listener->getPosition();
}

extern "C" _AnomalousExport void Listener_setPosition(Listener* listener, Vector3 value)
{
	listener->setPosition(value);
}

extern "C" _AnomalousExport Vector3 Listener_getVelocity(Listener* listener)
{
	return listener->getVelocity();
}

extern "C" _AnomalousExport void Listener_setVelocity(Listener* listener, Vector3 value)
{
	listener->setVelocity(value);
}

extern "C" _AnomalousExport void Listener_getOrientation(Listener* listener, Vector3* at, Vector3* up)
{
	listener->getOrientation(*at, *up);
}

extern "C" _AnomalousExport void Listener_setOrientation(Listener* listener, Vector3 at, Vector3 up)
{
	listener->setOrientation(at, up);
}