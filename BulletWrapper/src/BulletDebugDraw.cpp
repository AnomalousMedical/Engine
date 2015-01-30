#include "StdAfx.h"
#include "../Include/BulletDebugDraw.h"

int BulletDebugDraw::debugMode = 0;

BulletDebugDraw::BulletDebugDraw(DrawLineCallback drawLine, ReportErrorWarningCallback reportWarning HANDLE_ARG)
:drawLineCallback(drawLine),
reportWarningCallback(reportWarning)
ASSIGN_HANDLE_INITIALIZER
{

}

BulletDebugDraw::~BulletDebugDraw(void)
{

}

void BulletDebugDraw::drawLine(const btVector3& from,const btVector3& to,const btVector3& color)
{
	drawLineCallback(color, from, to PASS_HANDLE_ARG);
}

void BulletDebugDraw::drawContactPoint(const btVector3& PointOnB,const btVector3& normalOnB,btScalar distance,int lifeTime,const btVector3& color)
{
	btVector3 to = PointOnB + normalOnB * distance;
	const btVector3& from = PointOnB;
	drawLineCallback(color, from, to PASS_HANDLE_ARG);
}

void BulletDebugDraw::reportErrorWarning(const char* warningString)
{
	reportWarningCallback(warningString PASS_HANDLE_ARG);
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
}

void BulletDebugDraw::disableGlobalDebugMode(int mode)
{
	BulletDebugDraw::debugMode &= (~mode);
}

extern "C" _AnomalousExport BulletDebugDraw* BulletDebugDraw_Create(DrawLineCallback drawLine, ReportErrorWarningCallback reportWarning HANDLE_ARG)
{
	return new BulletDebugDraw(drawLine, reportWarning PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void BulletDebugDraw_Delete(BulletDebugDraw* debugDraw)
{
	delete debugDraw;
}

extern "C" _AnomalousExport void BulletDebugDraw_setGlobalDebugMode(int debugMode)
{
	BulletDebugDraw::setGlobalDebugMode(debugMode);
}

extern "C" _AnomalousExport void BulletDebugDraw_enableGlobalDebugMode(int debugMode)
{
	BulletDebugDraw::enableGlobalDebugMode(debugMode);
}

extern "C" _AnomalousExport void BulletDebugDraw_disableGlobalDebugMode(int debugMode)
{
	BulletDebugDraw::disableGlobalDebugMode(debugMode);
}

extern "C" _AnomalousExport int BulletDebugDraw_getGlobalDebugMode()
{
	return BulletDebugDraw::getGlobalDebugMode();
}