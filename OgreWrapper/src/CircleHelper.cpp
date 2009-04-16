#include "StdAfx.h"
#include "..\include\CircleHelper.h"
#include "NativeCircleHelper.h"
#include "SceneNode.h"
#include "MarshalUtils.h"
#include "ManualObject.h"
#include "Color.h"

namespace Rendering{

CircleHelper::CircleHelper(void)
:nativeSurface(new NativeCircleHelper())
{

}

void CircleHelper::drawCircle(Vector3 origin, Vector3 xAxis, Vector3 yAxis, float radius, int numLines)
{
	float nOrigin[] = {origin.x, origin.y, origin.z};
	float nXAxis[] = {xAxis.x, xAxis.y, xAxis.z};
	float nYAxis[] = {yAxis.x, yAxis.y, yAxis.z};
	nativeSurface->drawCircle(nOrigin, nXAxis, nYAxis, radius, numLines);
}

void CircleHelper::drawCircle(Vector3% origin, Vector3 xAxis, Vector3 yAxis, float radius, int numLines)
{
	float nOrigin[] = {origin.x, origin.y, origin.z};
	float nXAxis[] = {xAxis.x, xAxis.y, xAxis.z};
	float nYAxis[] = {yAxis.x, yAxis.y, yAxis.z};
	nativeSurface->drawCircle(nOrigin, nXAxis, nYAxis, radius, numLines);
}

void CircleHelper::drawCircle(Vector3% origin, Vector3% xAxis, Vector3% yAxis, float radius, int numLines)
{
	float nOrigin[] = {origin.x, origin.y, origin.z};
	float nXAxis[] = {xAxis.x, xAxis.y, xAxis.z};
	float nYAxis[] = {yAxis.x, yAxis.y, yAxis.z};
	nativeSurface->drawCircle(nOrigin, nXAxis, nYAxis, radius, numLines);
}

void CircleHelper::setColor(float r, float g, float b, float a)
{
	nativeSurface->setColor(r, g, b, a);
}

void CircleHelper::setColor(Color color)
{
	nativeSurface->setColor(color.r, color.g, color.b, color.a);
}

void CircleHelper::clear()
{
	nativeSurface->clear();
}

void CircleHelper::begin()
{
	nativeSurface->begin();
}

void CircleHelper::end()
{
	nativeSurface->end();
}

void CircleHelper::attachToNode(SceneNode^ node)
{
	nativeSurface->attachToNode(node->getSceneNode());
}

void CircleHelper::detachFromNode(SceneNode^ node)
{
	nativeSurface->detachFromNode(node->getSceneNode());
}

void CircleHelper::setBuffer(ManualObject^ manualObject)
{
	if(manualObject != nullptr)
	{
		nativeSurface->setBuffer(manualObject->getManualObject());
	}
	else
	{
		Logging::Log::Default->sendMessage("ManualObject sent as buffer to circle helper was null.", Logging::LogLevel::Warning, "Rendering");
	}
}

void CircleHelper::clearBuffer()
{
	nativeSurface->clearBuffer();
}

NativeCircleHelper* CircleHelper::getLineHelper()
{
	return nativeSurface.Get();
}

void CircleHelper::setVisible(bool visible)
{
	nativeSurface->setVisible(visible);
}

void CircleHelper::setMaterial(System::String^ material)
{
	nativeSurface->setMaterial(MarshalUtils::convertString(material));
}

void CircleHelper::setRenderQueueGroup(unsigned char queueID)
{
	nativeSurface->setRenderQueueGroup(queueID);
}

unsigned char CircleHelper::getRenderQueueGroup()
{
	return nativeSurface->getRenderQueueGroup();
}

}