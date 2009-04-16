#include "StdAfx.h"
#include "..\include\TriangleHelper.h"
#include "NativeTriangleHelper.h"
#include "SceneNode.h"
#include "MarshalUtils.h"
#include "ManualObject.h"
#include "Color.h"

namespace Rendering{

TriangleHelper::TriangleHelper()
:nativeSurface(new NativeTriangleHelper())
{

}

void TriangleHelper::drawTriangle(Vector3% p1, Vector3% p2, Vector3% p3)
{
	float p1a[] = {p1.x, p1.y, p1.z};
	float p2a[] = {p2.x, p2.y, p2.z};
	float p3a[] = {p3.x, p3.y, p3.z};
	nativeSurface->drawTriangle(p1a, p2a, p3a);
}

void TriangleHelper::drawTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
{
	float p1a[] = {p1.x, p1.y, p1.z};
	float p2a[] = {p2.x, p2.y, p2.z};
	float p3a[] = {p3.x, p3.y, p3.z};
	nativeSurface->drawTriangle(p1a, p2a, p3a);
}

void TriangleHelper::setColor(float r, float g, float b, float a)
{
	nativeSurface->setColor(r, g, b, a);
}

void TriangleHelper::setColor(Color color)
{
	nativeSurface->setColor(color.r, color.g, color.b, color.a);
}

void TriangleHelper::clear()
{
	nativeSurface->clear();
}

void TriangleHelper::begin()
{
	nativeSurface->begin();
}

void TriangleHelper::end()
{
	nativeSurface->end();
}

void TriangleHelper::attachToNode(SceneNode^ node)
{
	nativeSurface->attachToNode(node->getSceneNode());
}

void TriangleHelper::detachFromNode(SceneNode^ node)
{
	nativeSurface->detachFromNode(node->getSceneNode());
}

void TriangleHelper::setBuffer(ManualObject^ manualObject)
{
	if(manualObject != nullptr)
	{
		nativeSurface->setBuffer(manualObject->getManualObject());
	}
	else
	{
		Logging::Log::Default->sendMessage("ManualObject sent as buffer to triangle helper was null.", Logging::LogLevel::Warning, "Rendering");
	}
}

void TriangleHelper::clearBuffer()
{
	nativeSurface->clearBuffer();
}

NativeTriangleHelper* TriangleHelper::getTriangleHelper()
{
	return nativeSurface.Get();
}

void TriangleHelper::setVisible(bool visible)
{
	nativeSurface->setVisible(visible);
}

void TriangleHelper::setMaterial(System::String^ material)
{
	nativeSurface->setMaterial(MarshalUtils::convertString(material));
}

void TriangleHelper::setRenderQueueGroup(unsigned char queueID)
{
	nativeSurface->setRenderQueueGroup(queueID);
}

unsigned char TriangleHelper::getRenderQueueGroup()
{
	return nativeSurface->getRenderQueueGroup();
}

}