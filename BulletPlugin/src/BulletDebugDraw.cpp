#include "StdAfx.h"
#include "..\include\BulletDebugDraw.h"

using namespace Engine;

namespace BulletPlugin
{

int BulletDebugDraw::debugMode = 0;

BulletDebugDraw::BulletDebugDraw(void)
{
}

BulletDebugDraw::~BulletDebugDraw(void)
{
}

void BulletDebugDraw::drawLine(const btVector3& from,const btVector3& to,const btVector3& color)
{
	drawingSurface->setColor(Color(color.x(), color.y(), color.z()));
	drawingSurface->drawLine(Vector3(from.x(), from.y(), from.z()), Vector3(to.x(), to.y(), to.z()));
}

void BulletDebugDraw::drawContactPoint(const btVector3& PointOnB,const btVector3& normalOnB,btScalar distance,int lifeTime,const btVector3& color)
{
	
}

void BulletDebugDraw::reportErrorWarning(const char* warningString)
{
	Logging::Log::Default->sendMessage(gcnew System::String(warningString), Logging::LogLevel::Warning, "Bullet");
}

void BulletDebugDraw::draw3dText(const btVector3& location,const char* textString)
{

}

void BulletDebugDraw::setDebugMode(int debugMode)
{
	BulletDebugDraw::debugMode = debugMode;
}

int	BulletDebugDraw::getDebugMode() const
{
	return BulletDebugDraw::debugMode;
}

void BulletDebugDraw::setDrawingSurface(gcroot<DebugDrawingSurface^> drawingSurface)
{
	this->drawingSurface = drawingSurface;
}

int BulletDebugDraw::getGlobalDebugMode()
{
	return BulletDebugDraw::debugMode;
}

void BulletDebugDraw::setGlobalDebugMode(int mode)
{
	BulletDebugDraw::debugMode = debugMode;
}

void BulletDebugDraw::enableGlobalDebugMode(int mode)
{
	BulletDebugDraw::debugMode |= mode;
	Logging::Log::Default->debug("Enabled {0}", debugMode);
}

void BulletDebugDraw::disableGlobalDebugMode(int mode)
{
	BulletDebugDraw::debugMode &= (~mode);
	Logging::Log::Default->debug("Disabled {0}", debugMode);
}

}