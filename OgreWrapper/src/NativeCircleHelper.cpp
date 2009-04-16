#include "StdAfx.h"
#include "..\include\NativeCircleHelper.h"

#pragma unmanaged

#include "Ogre.h"
#include <math.h>

namespace Rendering{

#define PI 3.14159265f

NativeCircleHelper::NativeCircleHelper(void)
:buffer(0),
r(1.0f),
g(0.0f),
b(0.0f),
a(1.0f),
material("colorvertex")
{
}

NativeCircleHelper::~NativeCircleHelper(void)
{
}

void NativeCircleHelper::drawCircle(float* origin, float* xAxis, float* yAxis, float radius, int numLines)
{
	//x means x in the first 45 degree octant
	//y means y in the first 45 degree octant
	//first octant is 0 to 45 degrees the remaining octants are defined counterclockwise
	//from there

	int i;
	int numLinesIn45 = numLines / 8;
	float angleDelta = PI / 4.0f / (float)numLinesIn45;
	//Increase by 1 to include 45
	++numLinesIn45;
	float* xAxisLines = new float[numLinesIn45];
	float* yAxisLines = new float[numLinesIn45];
	float currentAngle = 0.0f;
	for(i = 0; i < numLinesIn45; ++i)
	{
		xAxisLines[i] = cos(currentAngle) * radius;
		yAxisLines[i] = sin(currentAngle) * radius;
		currentAngle += angleDelta;
	}

	//first octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * xAxis[0] + yAxisLines[i] * yAxis[0] + origin[0],
						 xAxisLines[i] * xAxis[1] + yAxisLines[i] * yAxis[1] + origin[1],
						 xAxisLines[i] * xAxis[2] + yAxisLines[i] * yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * xAxis[0] + yAxisLines[i-1] * yAxis[0] + origin[0],
						 xAxisLines[i-1] * xAxis[1] + yAxisLines[i-1] * yAxis[1] + origin[1],
						 xAxisLines[i-1] * xAxis[2] + yAxisLines[i-1] * yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//second octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * yAxis[0] + yAxisLines[i] * xAxis[0] + origin[0],
						 xAxisLines[i] * yAxis[1] + yAxisLines[i] * xAxis[1] + origin[1],
						 xAxisLines[i] * yAxis[2] + yAxisLines[i] * xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * yAxis[0] + yAxisLines[i-1] * xAxis[0] + origin[0],
						 xAxisLines[i-1] * yAxis[1] + yAxisLines[i-1] * xAxis[1] + origin[1],
						 xAxisLines[i-1] * yAxis[2] + yAxisLines[i-1] * xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//third octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * -yAxis[0] + yAxisLines[i] * xAxis[0] + origin[0],
						 xAxisLines[i] * -yAxis[1] + yAxisLines[i] * xAxis[1] + origin[1],
						 xAxisLines[i] * -yAxis[2] + yAxisLines[i] * xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * -yAxis[0] + yAxisLines[i-1] * xAxis[0] + origin[0],
						 xAxisLines[i-1] * -yAxis[1] + yAxisLines[i-1] * xAxis[1] + origin[1],
						 xAxisLines[i-1] * -yAxis[2] + yAxisLines[i-1] * xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//fourth octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * -xAxis[0] + yAxisLines[i] * yAxis[0] + origin[0],
						 xAxisLines[i] * -xAxis[1] + yAxisLines[i] * yAxis[1] + origin[1],
						 xAxisLines[i] * -xAxis[2] + yAxisLines[i] * yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * -xAxis[0] + yAxisLines[i-1] * yAxis[0] + origin[0],
						 xAxisLines[i-1] * -xAxis[1] + yAxisLines[i-1] * yAxis[1] + origin[1],
						 xAxisLines[i-1] * -xAxis[2] + yAxisLines[i-1] * yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//fifth octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * -xAxis[0] + yAxisLines[i] * -yAxis[0] + origin[0],
						 xAxisLines[i] * -xAxis[1] + yAxisLines[i] * -yAxis[1] + origin[1],
						 xAxisLines[i] * -xAxis[2] + yAxisLines[i] * -yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * -xAxis[0] + yAxisLines[i-1] * -yAxis[0] + origin[0],
						 xAxisLines[i-1] * -xAxis[1] + yAxisLines[i-1] * -yAxis[1] + origin[1],
						 xAxisLines[i-1] * -xAxis[2] + yAxisLines[i-1] * -yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//sixth octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * -yAxis[0] + yAxisLines[i] * -xAxis[0] + origin[0],
						 xAxisLines[i] * -yAxis[1] + yAxisLines[i] * -xAxis[1] + origin[1],
						 xAxisLines[i] * -yAxis[2] + yAxisLines[i] * -xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * -yAxis[0] + yAxisLines[i-1] * -xAxis[0] + origin[0],
						 xAxisLines[i-1] * -yAxis[1] + yAxisLines[i-1] * -xAxis[1] + origin[1],
						 xAxisLines[i-1] * -yAxis[2] + yAxisLines[i-1] * -xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//seventh octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * yAxis[0] + yAxisLines[i] * -xAxis[0] + origin[0],
						 xAxisLines[i] * yAxis[1] + yAxisLines[i] * -xAxis[1] + origin[1],
						 xAxisLines[i] * yAxis[2] + yAxisLines[i] * -xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * yAxis[0] + yAxisLines[i-1] * -xAxis[0] + origin[0],
						 xAxisLines[i-1] * yAxis[1] + yAxisLines[i-1] * -xAxis[1] + origin[1],
						 xAxisLines[i-1] * yAxis[2] + yAxisLines[i-1] * -xAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	//eigth octant
	for(i = numLinesIn45 - 1; i != 0; --i)
	{
		buffer->position(xAxisLines[i] * xAxis[0] + yAxisLines[i] * -yAxis[0] + origin[0],
						 xAxisLines[i] * xAxis[1] + yAxisLines[i] * -yAxis[1] + origin[1],
						 xAxisLines[i] * xAxis[2] + yAxisLines[i] * -yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
		buffer->position(xAxisLines[i-1] * xAxis[0] + yAxisLines[i-1] * -yAxis[0] + origin[0],
						 xAxisLines[i-1] * xAxis[1] + yAxisLines[i-1] * -yAxis[1] + origin[1],
						 xAxisLines[i-1] * xAxis[2] + yAxisLines[i-1] * -yAxis[2] + origin[2]);
		buffer->colour(r, g, b, a);
	}

	delete[] xAxisLines;
	delete[] yAxisLines;
}

void NativeCircleHelper::setBuffer(Ogre::ManualObject* manualObject)
{
	this->buffer = manualObject;
}

void NativeCircleHelper::clearBuffer()
{
	this->buffer = 0;
}

void NativeCircleHelper::setColor(float r, float g, float b, float a)
{
	this->r = r;
	this->g = g;
	this->b = b;
	this->a = a;
}

void NativeCircleHelper::clear()
{
	buffer->clear();
}

void NativeCircleHelper::begin()
{
	buffer->begin(material, Ogre::RenderOperation::OT_LINE_LIST);
}

void NativeCircleHelper::end()
{
	buffer->end();
}

void NativeCircleHelper::attachToNode(Ogre::SceneNode* node)
{
	node->attachObject(buffer);
}

void NativeCircleHelper::detachFromNode(Ogre::SceneNode* node)
{
	node->detachObject(buffer);
}

void NativeCircleHelper::setVisible(bool visible)
{
	buffer->setVisible(visible);
}

void NativeCircleHelper::setMaterial(std::string material)
{
	this->material = material;
}

void NativeCircleHelper::setRenderQueueGroup(unsigned char queueID)
{
	buffer->setRenderQueueGroup(queueID);
}

unsigned char NativeCircleHelper::getRenderQueueGroup()
{
	return buffer->getRenderQueueGroup();
}

}

#pragma managed