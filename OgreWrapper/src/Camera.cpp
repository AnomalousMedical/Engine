/// <file>Camera.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\Camera.h"

#include "Ogre.h"
#include "MathUtils.h"

namespace Rendering{

Camera::Camera(Ogre::Camera* camera, System::String^ name)
:MovableObject(camera), 
camera( camera ), 
name( name ), 
camRoot(new CameraGCRoot())
{
	*(camRoot.Get()) = this;
	userDefinedObj.Reset(new VoidUserDefinedObject(CAMERA_GCROOT, camRoot.Get()));
	camera->setUserObject(userDefinedObj.Get());
}

Camera::~Camera()
{
	camera = 0;
}

Ogre::Camera* Camera::getCamera()
{
	return camera;
}

System::String^ Camera::getName()
{
	return name;
}

void Camera::setPosition( EngineMath::Vector3 position )
{
	camera->setPosition( MathUtils::copyVector3(position) );
}

EngineMath::Vector3 Camera::getPosition()
{
	return MathUtils::copyVector3(camera->getPosition());
}

EngineMath::Quaternion Camera::getDerivedOrientation()
{
	return MathUtils::copyQuaternion(camera->getDerivedOrientation());
}

EngineMath::Vector3 Camera::getDerivedPosition()
{
	return MathUtils::copyVector3(camera->getDerivedPosition());
}

EngineMath::Vector3 Camera::getDerivedDirection()
{
	return MathUtils::copyVector3(camera->getDerivedDirection());
}

EngineMath::Vector3 Camera::getDerivedUp()
{
	return MathUtils::copyVector3(camera->getDerivedUp());
}

EngineMath::Vector3 Camera::getDerivedRight()
{
	return MathUtils::copyVector3(camera->getDerivedRight());
}

EngineMath::Quaternion Camera::getRealOrientation()
{
	return MathUtils::copyQuaternion(camera->getRealOrientation());
}

EngineMath::Vector3 Camera::getRealPosition()
{
	return MathUtils::copyVector3(camera->getRealPosition());
}

EngineMath::Vector3 Camera::getRealDirection()
{
	return MathUtils::copyVector3(camera->getRealDirection());
}

EngineMath::Vector3 Camera::getRealUp()
{
	return MathUtils::copyVector3(camera->getRealUp());
}

EngineMath::Vector3 Camera::getRealRight()
{
	return MathUtils::copyVector3(camera->getRealRight());
}

void Camera::lookAt( EngineMath::Vector3 lookAt )
{
	camera->lookAt( MathUtils::copyVector3(lookAt) );
}

void Camera::setPolygonMode(PolygonMode mode)
{
	camera->setPolygonMode((Ogre::PolygonMode)mode);
}

PolygonMode Camera::getPolygonMode()
{
	return (PolygonMode)camera->getPolygonMode();
}

void Camera::setDirection(float x, float y, float z)
{
	camera->setDirection(x, y, z);
}

void Camera::setDirection(EngineMath::Vector3 direction)
{
	camera->setDirection(MathUtils::copyVector3(direction));
}

void Camera::getDirection(EngineMath::Vector3% direction)
{
	MathUtils::copyVector3(camera->getDirection(), direction);
}

void Camera::getUp(EngineMath::Vector3% up)
{
	MathUtils::copyVector3(camera->getUp(), up);
}

void Camera::getRight(EngineMath::Vector3% right)
{
	MathUtils::copyVector3(camera->getRight(), right);
}

void Camera::setLodBias(float factor)
{
	camera->setLodBias(factor);
}

float Camera::getLodBias()
{
	return camera->getLodBias();
}

EngineMath::Ray3 Camera::getCameraToViewportRay(float screenx, float screeny)
{
	EngineMath::Ray3 ray = EngineMath::Ray3();
	MathUtils::copyRay(camera->getCameraToViewportRay(screenx, screeny), ray);
	return ray;
}

void Camera::setWindow(float left, float top, float right, float bottom)
{
	camera->setWindow(left, top, right, bottom);
}

void Camera::resetWindow()
{
	camera->resetWindow();
}

bool Camera::isWindowSet()
{
	return camera->isWindowSet();
}

void Camera::setAutoAspectRatio(bool autoRatio)
{
	camera->setAutoAspectRatio(autoRatio);
}

bool Camera::getAutoAspectRatio()
{
	return camera->getAutoAspectRatio();
}

float Camera::getNearClipDistance()
{
	return camera->getNearClipDistance();
}

float Camera::getFarClipDistance()
{
	return camera->getFarClipDistance();
}

void Camera::setUseRenderingDistance(bool use)
{
	camera->setUseRenderingDistance(use);
}

bool Camera::getUseRenderingDistance()
{
	return camera->getUseRenderingDistance();
}

void Camera::setFOVy(float fovy)
{
	camera->setFOVy(Ogre::Degree(fovy));
}

float Camera::getFOVy()
{
	return camera->getFOVy().valueDegrees();
}

void Camera::setNearClipDistance(float nearDistance)
{
	camera->setNearClipDistance(nearDistance);
}

void Camera::setFarClipDistance(float farDistance)
{
	camera->setFarClipDistance(farDistance);
}

void Camera::setAspectRatio(float ratio)
{
	camera->setAspectRatio(ratio);
}

float Camera::getAspectRatio()
{
	return camera->getAspectRatio();
}

void Camera::setRenderingDistance(float dist)
{
	camera->setRenderingDistance(dist);
}

float Camera::getRenderingDistance()
{
	return camera->getRenderingDistance();
}

ProjectionType Camera::getProjectionType()
{
	return (ProjectionType)camera->getProjectionType();
}

void Camera::setProjectionType(ProjectionType type)
{
	camera->setProjectionType((Ogre::ProjectionType)type);
}

void Camera::setOrthoWindow(float w, float h)
{
	camera->setOrthoWindow(w, h);
}

void Camera::setOrthoWindowWidth(float w)
{
	camera->setOrthoWindowWidth(w);
}

void Camera::setOrthoWindowHeight(float h)
{
	camera->setOrthoWindowHeight(h);
}

float Camera::getOrthoWindowWidth()
{
	return camera->getOrthoWindowWidth();
}

float Camera::getOrthoWindowHeight()
{
	return camera->getOrthoWindowHeight();
}

}