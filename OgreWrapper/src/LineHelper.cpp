#include "StdAfx.h"
#include "..\include\LineHelper.h"
#include "NativeLineHelper.h"
#include "SceneNode.h"
#include "MarshalUtils.h"
#include "ManualObject.h"
#include "Color.h"

namespace Rendering{

LineHelper::LineHelper()
:nativeSurface(new NativeLineHelper())
{

}

void LineHelper::drawLine(Vector3% p1, Vector3% p2)
{
	float p1a[] = {p1.x, p1.y, p1.z};
	float p2a[] = {p2.x, p2.y, p2.z};
	nativeSurface->drawLine(p1a, p2a);
}

void LineHelper::drawLine(Vector3 p1, Vector3 p2)
{
	float p1a[] = {p1.x, p1.y, p1.z};
	float p2a[] = {p2.x, p2.y, p2.z};
	nativeSurface->drawLine(p1a, p2a);
}

void LineHelper::setColor(float r, float g, float b, float a)
{
	nativeSurface->setColor(r, g, b, a);
}

void LineHelper::setColor(Color color)
{
	nativeSurface->setColor(color.r, color.g, color.b, color.a);
}

void LineHelper::clear()
{
	nativeSurface->clear();
}

void LineHelper::begin()
{
	nativeSurface->begin();
}

void LineHelper::end()
{
	nativeSurface->end();
}

void LineHelper::attachToNode(SceneNode^ node)
{
	nativeSurface->attachToNode(node->getSceneNode());
}

void LineHelper::detachFromNode(SceneNode^ node)
{
	nativeSurface->detachFromNode(node->getSceneNode());
}

void LineHelper::setBuffer(ManualObject^ manualObject)
{
	if(manualObject != nullptr)
	{
		nativeSurface->setBuffer(manualObject->getManualObject());
	}
	else
	{
		Logging::Log::Default->sendMessage("ManualObject sent as buffer to line helper was null.", Logging::LogLevel::Warning, "Rendering");
	}
}

void LineHelper::clearBuffer()
{
	nativeSurface->clearBuffer();
}

NativeLineHelper* LineHelper::getLineHelper()
{
	return nativeSurface.Get();
}

void LineHelper::setVisible(bool visible)
{
	nativeSurface->setVisible(visible);
}

void LineHelper::setMaterial(System::String^ material)
{
	nativeSurface->setMaterial(MarshalUtils::convertString(material));
}

void LineHelper::setRenderQueueGroup(unsigned char queueID)
{
	nativeSurface->setRenderQueueGroup(queueID);
}

unsigned char LineHelper::getRenderQueueGroup()
{
	return nativeSurface->getRenderQueueGroup();
}

}