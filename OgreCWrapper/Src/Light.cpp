#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" __declspec(dllexport) void Light_setType(Ogre::Light* light, Ogre::Light::LightTypes type)
{
	light->setType(type);
}

extern "C" __declspec(dllexport) Ogre::Light::LightTypes Light_getType(Ogre::Light* light)
{
	return light->getType();
}

extern "C" __declspec(dllexport) void Light_setDiffuseColorRaw(Ogre::Light* light, float red, float green, float blue)
{
	light->setDiffuseColour(red, green, blue);
}

extern "C" __declspec(dllexport) void Light_setDiffuseColor(Ogre::Light* light, Color color)
{
	light->setDiffuseColour(color.toOgre());
}

extern "C" __declspec(dllexport) Color Light_getDiffuseColor(Ogre::Light* light)
{
	return light->getDiffuseColour();
}

extern "C" __declspec(dllexport) void Light_setSpecularColorRaw(Ogre::Light* light, float red, float green, float blue)
{
	light->setSpecularColour(red, green, blue);
}

extern "C" __declspec(dllexport) void Light_setSpecularColor(Ogre::Light* light, Color color)
{
	light->setSpecularColour(color.toOgre());
}

extern "C" __declspec(dllexport) Color Light_getSpecularColor(Ogre::Light* light)
{
	return light->getSpecularColour();
}

extern "C" __declspec(dllexport) void Light_setAttenuation(Ogre::Light* light, float range, float constant, float linear, float quadratic)
{
	light->setAttenuation(range, constant, linear, quadratic);
}

extern "C" __declspec(dllexport) float Light_getAttenuationRange(Ogre::Light* light)
{
	return light->getAttenuationRange();
}

extern "C" __declspec(dllexport) float Light_getAttenuationConstant(Ogre::Light* light)
{
	return light->getAttenuationConstant();
}

extern "C" __declspec(dllexport) float Light_getAttenuationLinear(Ogre::Light* light)
{
	return light->getAttenuationLinear();
}

extern "C" __declspec(dllexport) float Light_getAttenuationQuadric(Ogre::Light* light)
{
	return light->getAttenuationQuadric();
}

extern "C" __declspec(dllexport) void Light_setPosition(Ogre::Light* light, Vector3 pos)
{
	light->setPosition(pos.toOgre());
}

extern "C" __declspec(dllexport) void Light_setPositionRaw(Ogre::Light* light, float x, float y, float z)
{
	light->setPosition(x, y, z);
}

extern "C" __declspec(dllexport) Vector3 Light_getPosition(Ogre::Light* light)
{
	return light->getPosition();
}

extern "C" __declspec(dllexport) void Light_setDirectionRaw(Ogre::Light* light, float x, float y, float z)
{
	light->setDirection(x, y, z);
}

extern "C" __declspec(dllexport) void Light_setDirection(Ogre::Light* light, Vector3 dir)
{
	light->setDirection(dir.toOgre());
}

extern "C" __declspec(dllexport) Vector3 Light_getDirection(Ogre::Light* light)
{
	return light->getDirection();
}

extern "C" __declspec(dllexport) void Light_setSpotlightRange(Ogre::Light* light, float innerAngleRad, float outerAngleRad, float falloff)
{
	light->setSpotlightRange(Ogre::Radian(innerAngleRad), Ogre::Radian(outerAngleRad), falloff);
}

extern "C" __declspec(dllexport) float Light_getSpotlightInnerAngle(Ogre::Light* light)
{
	return light->getSpotlightInnerAngle().valueRadians();
}

extern "C" __declspec(dllexport) float Light_getSpotlightOuterAngle(Ogre::Light* light)
{
	return light->getSpotlightOuterAngle().valueRadians();
}

extern "C" __declspec(dllexport) float Light_getSpotlightFalloff(Ogre::Light* light)
{
	return light->getSpotlightFalloff();
}

extern "C" __declspec(dllexport) void Light_setSpotlightInnerAngle(Ogre::Light* light, float innerAngleRad)
{
	light->setSpotlightInnerAngle(Ogre::Radian(innerAngleRad));
}

extern "C" __declspec(dllexport) void Light_setSpotlightOuterAngle(Ogre::Light* light, float outerAngleRad)
{
	light->setSpotlightOuterAngle(Ogre::Radian(outerAngleRad));
}

extern "C" __declspec(dllexport) void Light_setSpotlightFalloff(Ogre::Light* light, float value)
{
	light->setSpotlightFalloff(value);
}

extern "C" __declspec(dllexport) void Light_setPowerScale(Ogre::Light* light, float power)
{
	light->setPowerScale(power);
}

extern "C" __declspec(dllexport) float Light_getPowerScale(Ogre::Light* light)
{
	return light->getPowerScale();
}

extern "C" __declspec(dllexport) void Light_setCastShadows(Ogre::Light* light, bool cast)
{
	light->setCastShadows(cast);
}

extern "C" __declspec(dllexport) bool Light_getCastShadows(Ogre::Light* light)
{
	return light->getCastShadows();
}

#pragma warning(pop)