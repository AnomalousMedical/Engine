#include "StdAfx.h"
#include "..\include\PhysBodyDesc.h"
#include "NxBodyDesc.h"

#include "MathUtil.h"

namespace PhysXWrapper
{

PhysBodyDesc::PhysBodyDesc(void)
:bodyDesc(new NxBodyDesc())
{

}

Engine::Vector3 PhysBodyDesc::MassSpaceInertia::get() 
{ 
	return MathUtil::copyVector3(bodyDesc->massSpaceInertia);
}

void PhysBodyDesc::MassSpaceInertia::set(Engine::Vector3 inertia) 
{ 
	MathUtil::copyVector3(inertia, bodyDesc->massSpaceInertia); 
}

float PhysBodyDesc::Mass::get() 
{ 
	return bodyDesc->mass; 
}

void PhysBodyDesc::Mass::set(float mass) 
{ 
	bodyDesc->mass = mass; 
}

Engine::Vector3 PhysBodyDesc::LinearVelocity::get() 
{ 
	return MathUtil::copyVector3(bodyDesc->linearVelocity);
}

void PhysBodyDesc::LinearVelocity::set(Engine::Vector3 linearVelocity) 
{
	MathUtil::copyVector3(linearVelocity, bodyDesc->linearVelocity);
}

Engine::Vector3 PhysBodyDesc::AngularVelocity::get() 
{ 
	return MathUtil::copyVector3(bodyDesc->angularVelocity);
}

void PhysBodyDesc::AngularVelocity::set(Engine::Vector3 angularVelocity) 
{
	MathUtil::copyVector3(angularVelocity, bodyDesc->angularVelocity); 
}

float PhysBodyDesc::WakeUpCounter::get() 
{
	return bodyDesc->wakeUpCounter; 
}

void PhysBodyDesc::WakeUpCounter::set(float wakeUpCounter) 
{ 
	bodyDesc->wakeUpCounter = wakeUpCounter; 
}

float PhysBodyDesc::LinearDamping::get() 
{ 
	return bodyDesc->linearDamping; 
}

void PhysBodyDesc::LinearDamping::set(float linearDamping) 
{ 
	bodyDesc->linearDamping = linearDamping; 
}

float PhysBodyDesc::AngularDamping::get() 
{ 
	return bodyDesc->angularDamping; 
}

void PhysBodyDesc::AngularDamping::set(float angularDamping) 
{
	bodyDesc->angularDamping = angularDamping; 
}

float PhysBodyDesc::MaxAngularVelocity::get() 
{ 
	return bodyDesc->maxAngularVelocity; 
}

void PhysBodyDesc::MaxAngularVelocity::set(float maxAngularVelocity) 
{ 
	bodyDesc->maxAngularVelocity = maxAngularVelocity; 
}

float PhysBodyDesc::CCDMotionThreshold::get() 
{ 
	return bodyDesc->CCDMotionThreshold; 
}

void PhysBodyDesc::CCDMotionThreshold::set(float motionThreshold) 
{
	bodyDesc->CCDMotionThreshold = motionThreshold; 
}

BodyFlag PhysBodyDesc::Flags::get() 
{
	return (BodyFlag)bodyDesc->flags; 
}

void PhysBodyDesc::Flags::set(BodyFlag flags) 
{
	bodyDesc->flags = (NxBodyFlag)flags; 
}

float PhysBodyDesc::SleepLinearVelocity::get() 
{ 
	return bodyDesc->sleepLinearVelocity;
}

void PhysBodyDesc::SleepLinearVelocity::set(float sleepLinearVelocity) 
{ 
	bodyDesc->sleepLinearVelocity = sleepLinearVelocity; 
}

float PhysBodyDesc::SleepAngularVelocity::get() 
{ 
	return bodyDesc->sleepAngularVelocity; 
}

void PhysBodyDesc::SleepAngularVelocity::set(float sleepAngularVelocity) 
{ 
	bodyDesc->sleepAngularVelocity = sleepAngularVelocity; 
}

unsigned int PhysBodyDesc::SolverIterationCount::get() 
{ 
	return bodyDesc->solverIterationCount; 
}

void PhysBodyDesc::SolverIterationCount::set(unsigned int solverIterationCount) 
{ 
	bodyDesc->solverIterationCount = solverIterationCount; 
}

float PhysBodyDesc::SleepEnergyThreshold::get() 
{ 
	return bodyDesc->sleepEnergyThreshold; 
}

void PhysBodyDesc::SleepEnergyThreshold::set(float sleepEnergyThreshold) 
{ 
	bodyDesc->sleepEnergyThreshold = sleepEnergyThreshold; 
}

float PhysBodyDesc::SleepDamping::get() 
{ 
	return bodyDesc->sleepDamping; 
}

void PhysBodyDesc::SleepDamping::set(float sleepDamping) 
{
	bodyDesc->sleepDamping = sleepDamping; 
}

float PhysBodyDesc::ContactReportThreshold::get() 
{ 
	return bodyDesc->contactReportThreshold; 
}

void PhysBodyDesc::ContactReportThreshold::set(float contactReportThreshold) 
{ 
	bodyDesc->contactReportThreshold = contactReportThreshold; 
}

}