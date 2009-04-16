#include "StdAfx.h"
#include "..\include\NativeLineHelper.h"
#include "Ogre.h"

#pragma unmanaged

namespace Rendering{

NativeLineHelper::NativeLineHelper()
:lineBuffer(0),
r(1.0f),
g(0.0f),
b(0.0f),
a(1.0f),
material("colorvertex")
{
}

NativeLineHelper::~NativeLineHelper(void)
{
}

void NativeLineHelper::drawLine(float* p1, float* p2)
{
	lineBuffer->position(p1[0], p1[1], p1[2]);
	lineBuffer->colour(r, g, b, a);
	lineBuffer->position(p2[0], p2[1], p2[2]);
	lineBuffer->colour(r, g, b, a);
}

void NativeLineHelper::setBuffer(Ogre::ManualObject* manualObject)
{
	this->lineBuffer = manualObject;
}

void NativeLineHelper::clearBuffer()
{
	this->lineBuffer = 0;
}

void NativeLineHelper::setColor(float r, float g, float b, float a)
{
	this->r = r;
	this->g = g;
	this->b = b;
	this->a = a;
}

void NativeLineHelper::clear()
{
	lineBuffer->clear();
}

void NativeLineHelper::begin()
{
	lineBuffer->begin(material, Ogre::RenderOperation::OT_LINE_LIST);
}

void NativeLineHelper::end()
{
	lineBuffer->end();
}

void NativeLineHelper::attachToNode(Ogre::SceneNode* node)
{
	node->attachObject(lineBuffer);
}

void NativeLineHelper::detachFromNode(Ogre::SceneNode* node)
{
	node->detachObject(lineBuffer);
}

void NativeLineHelper::setVisible(bool visible)
{
	lineBuffer->setVisible(visible);
}

void NativeLineHelper::setMaterial(std::string material)
{
	this->material = material;
}

void NativeLineHelper::setRenderQueueGroup(unsigned char queueID)
{
	lineBuffer->setRenderQueueGroup(queueID);
}

unsigned char NativeLineHelper::getRenderQueueGroup()
{
	return lineBuffer->getRenderQueueGroup();
}

}

#pragma managed