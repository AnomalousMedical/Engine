#include "StdAfx.h"
#include "..\include\PointHelper.h"
#include "NativePointHelper.h"
#include "SceneNode.h"
#include "MarshalUtils.h"
#include "ManualObject.h"
#include "Color.h"

namespace OgreWrapper{

PointHelper::PointHelper()
:nativeSurface(new NativePointHelper())
{

}

void PointHelper::drawPoint(Vector3% point)
{
	float p[] = {point.x, point.y, point.z};
	nativeSurface->drawPoint(p);
}

void PointHelper::drawPoint(Vector3 point)
{
	float p[] = {point.x, point.y, point.z};
	nativeSurface->drawPoint(p);
}

void PointHelper::setColor(float r, float g, float b, float a)
{
	nativeSurface->setColor(r, g, b, a);
}

void PointHelper::setColor(Color color)
{
	nativeSurface->setColor(color.r, color.g, color.b, color.a);
}

void PointHelper::clear()
{
	nativeSurface->clear();
}

void PointHelper::begin()
{
	nativeSurface->begin();
}

void PointHelper::end()
{
	nativeSurface->end();
}

void PointHelper::attachToNode(SceneNode^ node)
{
	nativeSurface->attachToNode(node->getSceneNode());
}

void PointHelper::detachFromNode(SceneNode^ node)
{
	nativeSurface->detachFromNode(node->getSceneNode());
}

void PointHelper::setBuffer(ManualObject^ manualObject)
{
	if(manualObject != nullptr)
	{
		nativeSurface->setBuffer(manualObject->getManualObject());
	}
	else
	{
		Logging::Log::Default->sendMessage("ManualObject sent as buffer to point helper was null.", Logging::LogLevel::Warning, "Rendering");
	}
}

void PointHelper::clearBuffer()
{
	nativeSurface->clearBuffer();
}

NativePointHelper* PointHelper::getPointHelper()
{
	return nativeSurface.Get();
}

void PointHelper::setVisible(bool visible)
{
	nativeSurface->setVisible(visible);
}

void PointHelper::setMaterial(System::String^ material)
{
	nativeSurface->setMaterial(MarshalUtils::convertString(material));
}

void PointHelper::setRenderQueueGroup(unsigned char queueID)
{
	nativeSurface->setRenderQueueGroup(queueID);
}

unsigned char PointHelper::getRenderQueueGroup()
{
	return nativeSurface->getRenderQueueGroup();
}

}