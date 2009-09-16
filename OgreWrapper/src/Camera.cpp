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
#include "MarshalUtils.h"

namespace OgreWrapper{

Camera::Camera(Ogre::Camera* camera)
:MovableObject(camera), 
camera( camera ), 
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
	return MarshalUtils::convertString(camera->getName());
}

void Camera::setPosition( Engine::Vector3 position )
{
	camera->setPosition( MathUtils::copyVector3(position) );
}

Engine::Vector3 Camera::getPosition()
{
	return MathUtils::copyVector3(camera->getPosition());
}

Engine::Quaternion Camera::getDerivedOrientation()
{
	return MathUtils::copyQuaternion(camera->getDerivedOrientation());
}

Engine::Vector3 Camera::getDerivedPosition()
{
	return MathUtils::copyVector3(camera->getDerivedPosition());
}

Engine::Vector3 Camera::getDerivedDirection()
{
	return MathUtils::copyVector3(camera->getDerivedDirection());
}

Engine::Vector3 Camera::getDerivedUp()
{
	return MathUtils::copyVector3(camera->getDerivedUp());
}

Engine::Vector3 Camera::getDerivedRight()
{
	return MathUtils::copyVector3(camera->getDerivedRight());
}

Engine::Quaternion Camera::getRealOrientation()
{
	return MathUtils::copyQuaternion(camera->getRealOrientation());
}

Engine::Vector3 Camera::getRealPosition()
{
	return MathUtils::copyVector3(camera->getRealPosition());
}

Engine::Vector3 Camera::getRealDirection()
{
	return MathUtils::copyVector3(camera->getRealDirection());
}

Engine::Vector3 Camera::getRealUp()
{
	return MathUtils::copyVector3(camera->getRealUp());
}

Engine::Vector3 Camera::getRealRight()
{
	return MathUtils::copyVector3(camera->getRealRight());
}

void Camera::lookAt( Engine::Vector3 lookAt )
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

void Camera::setDirection(Engine::Vector3 direction)
{
	camera->setDirection(MathUtils::copyVector3(direction));
}

void Camera::getDirection(Engine::Vector3% direction)
{
	MathUtils::copyVector3(camera->getDirection(), direction);
}

void Camera::getUp(Engine::Vector3% up)
{
	MathUtils::copyVector3(camera->getUp(), up);
}

void Camera::getRight(Engine::Vector3% right)
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

Engine::Ray3 Camera::getCameraToViewportRay(float screenx, float screeny)
{
	Engine::Ray3 ray = Engine::Ray3();
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

Engine::Matrix4x4 Camera::getViewMatrix()
{
	return MathUtils::copyMatrix4x4(camera->getViewMatrix());
}

Engine::Matrix4x4 Camera::getProjectionMatrix()
{
	return MathUtils::copyMatrix4x4(camera->getProjectionMatrix());
}

}