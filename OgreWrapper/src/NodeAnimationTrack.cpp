#include "stdafx.h"
#include "NodeAnimationTrack.h"
#include "Ogre.h"
#include "TransformKeyFrame.h"

namespace OgreWrapper
{

NodeAnimationTrack::NodeAnimationTrack(Ogre::NodeAnimationTrack* ogreAnimation, Animation^ parent)
:AnimationTrack(ogreAnimation, parent), ogreAnimation(ogreAnimation)
{

}

NodeAnimationTrack::~NodeAnimationTrack()
{
	if(ogreAnimation != 0)
	{
		ogreAnimation = 0;
	}
}

TransformKeyFrame^ NodeAnimationTrack::createNodeKeyFrame(float timePos)
{
	throw gcnew System::NotImplementedException();
}

RenderNode^ NodeAnimationTrack::getAssociatedNode()
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::setAssociatedNode(RenderNode^ node)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::applyToNode(RenderNode^ node, TimeIndex timeIndex)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::applyToNode(RenderNode^ node, TimeIndex timeIndex, float weight)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::applyToNode(RenderNode^ node, TimeIndex timeIndex, float weight, float scale)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::applyToNode(RenderNode^ node, TimeIndex% timeIndex)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::applyToNode(RenderNode^ node, TimeIndex% timeIndex, float weight)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::applyToNode(RenderNode^ node, TimeIndex% timeIndex, float weight, float scale)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::setUseShortestRotationPath(bool useShortestPath)
{
	throw gcnew System::NotImplementedException();
}

bool NodeAnimationTrack::getUseShortestRotationPath()
{
	throw gcnew System::NotImplementedException();
}

KeyFrame^ NodeAnimationTrack::getInterpolatedKeyFrame(TimeIndex timeIndex)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::apply(TimeIndex timeIndex)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::apply(TimeIndex timeIndex, float weight)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::apply(TimeIndex timeIndex, float weight, float scale)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::apply(TimeIndex% timeIndex)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::apply(TimeIndex% timeIndex, float weight)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::apply(TimeIndex% timeIndex, float weight, float scale)
{
	throw gcnew System::NotImplementedException();
}

TransformKeyFrame^ NodeAnimationTrack::getNodeKeyFrame(unsigned short index)
{
	throw gcnew System::NotImplementedException();
}

KeyFrame^ NodeAnimationTrack::getKeyFrame(unsigned short index)
{
	throw gcnew System::NotImplementedException();
}

float NodeAnimationTrack::getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2)
{
	throw gcnew System::NotImplementedException();
}

float NodeAnimationTrack::getKeyFramesAtTime(TimeIndex% timeIndex, _OUT KeyFrame^ keyFrame1, _OUT KeyFrame^ keyFrame2, _OUT unsigned short% firstKeyIndex)
{
	throw gcnew System::NotImplementedException();
}

KeyFrame^ NodeAnimationTrack::createKeyFrame(float timePos)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::removeKeyFrame(unsigned short index)
{
	throw gcnew System::NotImplementedException();
}

void NodeAnimationTrack::removeAllKeyFrames()
{
	throw gcnew System::NotImplementedException();
}

}