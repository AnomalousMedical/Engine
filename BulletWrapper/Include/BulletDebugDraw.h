#pragma once

typedef void (*DrawLineCallback)(const Vector3& color, const Vector3& from, const Vector3& to);
typedef void (*ReportErrorWarningCallback)(const char* message);

class BulletDebugDraw : public btIDebugDraw
{
private:
	DrawLineCallback drawLineCallback;
	ReportErrorWarningCallback reportWarningCallback;
	static int debugMode;

public:
	BulletDebugDraw(DrawLineCallback drawLine, ReportErrorWarningCallback reportWarning);

	virtual ~BulletDebugDraw(void);

	virtual void drawLine(const btVector3& from, const btVector3& to, const btVector3& color);

	virtual void drawContactPoint(const btVector3& PointOnB, const btVector3& normalOnB, btScalar distance, int lifeTime, const btVector3& color);

	virtual void reportErrorWarning(const char* warningString);

	virtual void draw3dText(const btVector3& location, const char* textString);

	virtual void setDebugMode(int debugMode);

	virtual int	getDebugMode() const;

	static void setGlobalDebugMode(int mode);

	static void enableGlobalDebugMode(int mode);

	static void disableGlobalDebugMode(int mode);

	static int getGlobalDebugMode();
};
