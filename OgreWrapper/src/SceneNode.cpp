/// <file>SceneNode.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\SceneNode.h"
#include "SceneManager.h"
#include "MovableObject.h"

#include "Ogre.h"

namespace OgreWrapper{

SceneNode::SceneNode(Ogre::SceneNode* sceneNode, System::String^ name)
:RenderNode(sceneNode),
sceneNode( sceneNode ), 
name( name ),
nodeObjects(gcnew NodeObjectList())
{

}

SceneNode::SceneNode(System::String^ name, SceneManager^ ownerScene)
:name(name), 
autoOgreNode(new Ogre::SceneNode(ownerScene->getSceneManager())),
RenderNode(autoOgreNode.Get())
{
	sceneNode = autoOgreNode.Get();
}

SceneNode::~SceneNode()
{
	sceneNode = 0;
}

Ogre::SceneNode* SceneNode::getSceneNode()
{
	return sceneNode;
}

System::String^ SceneNode::getName()
{
	return name;
}

void SceneNode::addChild( SceneNode^ child )
{
	sceneNode->addChild( child->getSceneNode() );
}

void SceneNode::removeChild(SceneNode^ child)
{
	sceneNode->removeChild( child->getSceneNode() );
}

void SceneNode::attachObject( MovableObject^ object )
{
	sceneNode->attachObject( object->getMovableObject() );
	nodeObjects->Add(object->getName(), object);
}

void SceneNode::detachObject( MovableObject^ object )
{
	if(object != nullptr && nodeObjects->ContainsKey(object->getName()))
	{
		sceneNode->detachObject(object->getMovableObject());
		nodeObjects->Remove(object->getName());
	}
}

System::Collections::Generic::IEnumerable<MovableObject^>^ SceneNode::getNodeObjectIter()
{
	return nodeObjects->Values;
}

MovableObject^ SceneNode::getNodeObject(System::String^ name)
{
	if(nodeObjects->ContainsKey(name))
	{
		return nodeObjects[name];
	}
	return nullptr;
}

void SceneNode::setAutoTracking(bool enabled, SceneNode^ target, EngineMath::Vector3 offset)
{
	Ogre::Vector3 ogOffset(offset.x, offset.y, offset.z);
	sceneNode->setAutoTracking(enabled, target->getSceneNode(), ogOffset);
}

}