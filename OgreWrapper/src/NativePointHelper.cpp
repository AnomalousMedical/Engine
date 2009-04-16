#include "StdAfx.h"
#include "..\include\NativePointHelper.h"
#include "Ogre.h"

#pragma unmanaged

namespace Engine{

namespace Rendering{

NativePointHelper::NativePointHelper()
:pointBuffer(0),
r(1.0f),
g(0.0f),
b(0.0f),
a(1.0f),
material("colorvertex")
{
}

NativePointHelper::~NativePointHelper(void)
{
}

void NativePointHelper::drawPoint(float* p)
{
	pointBuffer->position(p[0], p[1], p[2]);
	pointBuffer->colour(r, g, b, a);
}

void NativePointHelper::setBuffer(Ogre::ManualObject* manualObject)
{
	this->pointBuffer = manualObject;
}

void NativePointHelper::clearBuffer()
{
	this->pointBuffer = 0;
}

void NativePointHelper::setColor(float r, float g, float b, float a)
{
	this->r = r;
	this->g = g;
	this->b = b;
	this->a = a;
}

void NativePointHelper::clear()
{
	pointBuffer->clear();
}

void NativePointHelper::begin()
{
	pointBuffer->begin(material, Ogre::RenderOperation::OT_LINE_LIST);
}

void NativePointHelper::end()
{
	pointBuffer->end();
}

void NativePointHelper::attachToNode(Ogre::SceneNode* node)
{
	node->attachObject(pointBuffer);
}

void NativePointHelper::detachFromNode(Ogre::SceneNode* node)
{
	node->detachObject(pointBuffer);
}

void NativePointHelper::setVisible(bool visible)
{
	pointBuffer->setVisible(visible);
}

void NativePointHelper::setMaterial(std::string material)
{
	this->material = material;
}

void NativePointHelper::setRenderQueueGroup(unsigned char queueID)
{
	pointBuffer->setRenderQueueGroup(queueID);
}

unsigned char NativePointHelper::getRenderQueueGroup()
{
	return pointBuffer->getRenderQueueGroup();
}

}

}

#pragma managed