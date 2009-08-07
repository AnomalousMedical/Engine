#include "StdAfx.h"
#include "..\include\BulletDebugEntry.h"
#include "BulletDebugDraw.h"

namespace BulletPlugin
{

BulletDebugEntry::BulletDebugEntry(String^ text, btIDebugDraw::DebugDrawModes mode)
:text(text),
mode(mode)
{
}

void BulletDebugEntry::setEnabled(bool enabled)
{
	if(enabled)
	{
		BulletDebugDraw::enableGlobalDebugMode(mode);
	}
	else
	{
		BulletDebugDraw::disableGlobalDebugMode(mode);
	}
}

}