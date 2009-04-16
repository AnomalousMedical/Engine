#include "StdAfx.h"
#include "..\include\NativeTriangleHelper.h"
#include "Ogre.h"

#pragma unmanaged

namespace OgreWrapper{

NativeTriangleHelper::NativeTriangleHelper()
:triangleBuffer(0),
r(1.0f),
g(0.0f),
b(0.0f),
a(1.0f),
material("colorvertex")
{
}

NativeTriangleHelper::~NativeTriangleHelper(void)
{
}

void NativeTriangleHelper::drawTriangle(float* p1, float* p2, float* p3)
{
	triangleBuffer->position(p1[0], p1[1], p1[2]);
	triangleBuffer->colour(r, g, b, a);
	triangleBuffer->position(p2[0], p2[1], p2[2]);
	triangleBuffer->colour(r, g, b, a);
	triangleBuffer->position(p3[0], p3[1], p3[2]);
	triangleBuffer->colour(r, g, b, a);
}

void NativeTriangleHelper::setBuffer(Ogre::ManualObject* manualObject)
{
	this->triangleBuffer = manualObject;
}

void NativeTriangleHelper::clearBuffer()
{
	this->triangleBuffer = 0;
}

void NativeTriangleHelper::setColor(float r, float g, float b, float a)
{
	this->r = r;
	this->g = g;
	this->b = b;
	this->a = a;
}

void NativeTriangleHelper::clear()
{
	triangleBuffer->clear();
}

void NativeTriangleHelper::begin()
{
	triangleBuffer->begin(material, Ogre::RenderOperation::OT_LINE_LIST);
}

void NativeTriangleHelper::end()
{
	triangleBuffer->end();
}

void NativeTriangleHelper::attachToNode(Ogre::SceneNode* node)
{
	node->attachObject(triangleBuffer);
}

void NativeTriangleHelper::detachFromNode(Ogre::SceneNode* node)
{
	node->detachObject(triangleBuffer);
}

void NativeTriangleHelper::setVisible(bool visible)
{
	triangleBuffer->setVisible(visible);
}

void NativeTriangleHelper::setMaterial(std::string material)
{
	this->material = material;
}

void NativeTriangleHelper::setRenderQueueGroup(unsigned char queueID)
{
	triangleBuffer->setRenderQueueGroup(queueID);
}

unsigned char NativeTriangleHelper::getRenderQueueGroup()
{
	return triangleBuffer->getRenderQueueGroup();
}

}

#pragma managed