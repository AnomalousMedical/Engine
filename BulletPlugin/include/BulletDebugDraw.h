#pragma once

#include "vcclr.h"

using namespace Engine::Renderer;

namespace BulletPlugin
{

class BulletDebugDraw : public btIDebugDraw
{
private:
	static int debugMode;
	gcroot<DebugDrawingSurface^> drawingSurface;

public:
	BulletDebugDraw(void);

	virtual ~BulletDebugDraw(void);

	virtual void drawLine(const btVector3& from, const btVector3& to, const btVector3& color);

	virtual void drawContactPoint(const btVector3& PointOnB, const btVector3& normalOnB, btScalar distance, int lifeTime, const btVector3& color);

	virtual void reportErrorWarning(const char* warningString);

	virtual void draw3dText(const btVector3& location, const char* textString);
	
	virtual void setDebugMode(int debugMode);
	
	virtual int	getDebugMode() const;

	void setDrawingSurface(gcroot<DebugDrawingSurface^> drawingSurface);

	static void setGlobalDebugMode(int mode);

	static void enableGlobalDebugMode(int mode);

	static void disableGlobalDebugMode(int mode);

	static int getGlobalDebugMode();
};

}