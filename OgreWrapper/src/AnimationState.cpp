#include "StdAfx.h"
#include "..\include\AnimationState.h"

namespace OgreWrapper{

AnimationState::AnimationState(Ogre::AnimationState* animationState, AnimationStateSet^ parent)
:animationState(animationState), parent(parent)
{

}

AnimationState::~AnimationState(void)
{

}

System::String^ AnimationState::getAnimationName()
{
	return gcnew System::String(animationState->getAnimationName().c_str());
}

float AnimationState::getTimePosition()
{
	return animationState->getTimePosition();
}

void AnimationState::setTimePosition(float timePos)
{
	animationState->setTimePosition(timePos);
}

float AnimationState::getLength()
{
	return animationState->getLength();
}

void AnimationState::setLength(float length)
{
	animationState->setLength(length);
}

float AnimationState::getWeight()
{
	return animationState->getWeight();
}

void AnimationState::setWeight(float weight)
{
	animationState->setWeight(weight);
}

void AnimationState::addTime(float offset)
{
	animationState->addTime(offset);
}

bool AnimationState::hasEnded()
{
	return animationState->hasEnded();
}

bool AnimationState::getEnabled()
{
	return animationState->getEnabled();
}

void AnimationState::setEnabled(bool enabled)
{
	animationState->setEnabled(enabled);
}

void AnimationState::setLoop(bool loop)
{
	animationState->setLoop(loop);
}

bool AnimationState::getLoop()
{
	return animationState->getLoop();
}

void AnimationState::copyStateFrom(AnimationState^ animationState)
{
	this->animationState->copyStateFrom(*animationState->animationState);
}

void AnimationState::createBlendMask(unsigned int blendMaskSizeHint, float initialWeight)
{
	animationState->createBlendMask(blendMaskSizeHint, initialWeight);
}

void AnimationState::destroyBlendMask()
{
	animationState->destroyBlendMask();
}

bool AnimationState::hasBlendMask()
{
	return animationState->hasBlendMask();
}

void AnimationState::setBlendMaskEntry(unsigned int boneHandle, float weight)
{
	animationState->setBlendMaskEntry(boneHandle, weight);
}

float AnimationState::getBlendMaskEntry(unsigned int boneHandle)
{
	return animationState->getBlendMaskEntry(boneHandle);
}

AnimationStateSet^ AnimationState::getParent()
{
	return parent;
}

}