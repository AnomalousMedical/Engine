#include "StdAfx.h"
#include "..\include\BulletDebugDraw.h"

namespace BulletPlugin
{

BulletDebugDraw::BulletDebugDraw(void)
:debugMode(0)
{
}

BulletDebugDraw::~BulletDebugDraw(void)
{
}

void BulletDebugDraw::drawLine(const btVector3& from,const btVector3& to,const btVector3& color)
{
	
}

void BulletDebugDraw::drawContactPoint(const btVector3& PointOnB,const btVector3& normalOnB,btScalar distance,int lifeTime,const btVector3& color)
{

}

void BulletDebugDraw::reportErrorWarning(const char* warningString)
{

}

void BulletDebugDraw::draw3dText(const btVector3& location,const char* textString)
{

}

void BulletDebugDraw::setDebugMode(int debugMode)
{
	this->debugMode = debugMode;
}

int	BulletDebugDraw::getDebugMode() const
{
	return debugMode;
}

}