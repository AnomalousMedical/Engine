/// <file>Light.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Light.h"

#include "Ogre.h"
#include "MathUtils.h"
#include "MarshalUtils.h"

namespace OgreWrapper
{

Light::Light(Ogre::Light* light)
:MovableObject(light), light( light )
{
}

Light::~Light()
{
	light = 0;
}

Ogre::Light* Light::getLight()
{
	return light;
}

System::String^ Light::getName()
{
	return MarshalUtils::convertString(light->getName());
}

void Light::setType(Light::LightTypes type)
{
	light->setType((Ogre::Light::LightTypes)type);
}

Light::LightTypes Light::getType()
{
	return (Light::LightTypes)light->getType();
}

void Light::setDiffuseColor(float red, float green, float blue)
{
	light->setDiffuseColour(red, green, blue);
}

void Light::setDiffuseColor(EngineMath::Vector3 color)
{
	light->setDiffuseColour(color.x, color.y, color.z);
}

void Light::getDiffuseColor(EngineMath::Vector3% color)
{
	Ogre::ColourValue cv = light->getDiffuseColour();
	color.x = cv.r;
	color.y = cv.g;
	color.z = cv.b;
}

void Light::setSpecularColor(float red, float green, float blue)
{
	light->setSpecularColour(red, green, blue);
}

void Light::setSpecularColor(EngineMath::Vector3 color)
{
	light->setSpecularColour(color.x, color.y, color.z);
}

void Light::getSpecularColor(EngineMath::Vector3% color)
{
	Ogre::ColourValue cv = light->getSpecularColour();
	color.x = cv.r;
	color.y = cv.g;
	color.z = cv.b;
}

void Light::setAttenuation(float range, float constant, float linear, float quadratic)
{
	light->setAttenuation(range, constant, linear, quadratic);
}

float Light::getAttenuationRange()
{
	return light->getAttenuationRange();
}

float Light::getAttenuationConstant()
{
	return light->getAttenuationConstant();
}

float Light::getAttenuationLinear()
{
	return light->getAttenuationLinear();
}

float Light::getAttenuationQuadric()
{
	return light->getAttenuationQuadric();
}

void Light::setDirection(float x, float y, float z)
{
	light->setDirection(x, y, z);
}

void Light::setDirection(EngineMath::Vector3 dir)
{
	light->setDirection(MathUtils::copyVector3(dir));
}

void Light::getDirection(EngineMath::Vector3% dir)
{
	MathUtils::copyVector3(light->getDirection(), dir);
}

void Light::setSpotlightRange(float innerAngleRad, float outerAngleRad, float falloff)
{
	light->setSpotlightRange(Ogre::Radian(innerAngleRad), Ogre::Radian(outerAngleRad), falloff);
}

float Light::getSpotlightInnerAngle()
{
	return light->getSpotlightInnerAngle().valueRadians();
}

float Light::getSpotlightOuterAngle()
{
	return light->getSpotlightOuterAngle().valueRadians();
}

float Light::getSpotlightFalloff()
{
	return light->getSpotlightFalloff();
}

void Light::setSpotlightInnerAngle(float innerAngleRad)
{
	light->setSpotlightInnerAngle(Ogre::Radian(innerAngleRad));
}

void Light::setSpotlightOuterAngle(float outerAngleRad)
{
	light->setSpotlightOuterAngle(Ogre::Radian(outerAngleRad));
}

void Light::setSpotlightFalloff(float value)
{
	light->setSpotlightFalloff(value);
}

void Light::setPowerScale(float power)
{
	light->setPowerScale(power);
}

float Light::getPowerScale()
{
	return light->getPowerScale();
}

}