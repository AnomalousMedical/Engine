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
#include "MarshalUtils.h"

#include "Ogre.h"

namespace OgreWrapper{

SceneNode::SceneNode(Ogre::SceneNode* sceneNode)
:Node(sceneNode),
sceneNode( sceneNode ), 
nodeObjects(gcnew NodeObjectList()),
sceneNodeRoot(new SceneNodeGCRoot(this))
{
	Ogre::Any userPtr;
	userPtr = (void*)sceneNodeRoot;
	sceneNode->setUserAny(userPtr);
}

SceneNode::SceneNode(System::String^ name, SceneManager^ ownerScene)
:autoOgreNode(new Ogre::SceneNode(ownerScene->getSceneManager(), MarshalUtils::convertString(name))),
Node(autoOgreNode.Get()),
nodeObjects(gcnew NodeObjectList()),
sceneNodeRoot(new SceneNodeGCRoot(this))
{
	sceneNode = autoOgreNode.Get();
	Ogre::Any userPtr;
	userPtr = (void*)sceneNodeRoot;
	sceneNode->setUserAny(userPtr);
}

SceneNode::~SceneNode()
{
	if(sceneNodeRoot != 0)
	{
		delete sceneNodeRoot;
		sceneNodeRoot = 0;
	}
	sceneNode = 0;
}

Ogre::SceneNode* SceneNode::getSceneNode()
{
	return sceneNode;
}

System::String^ SceneNode::getName()
{
	return MarshalUtils::convertString(sceneNode->getName());
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

void SceneNode::setAutoTracking(bool enabled, SceneNode^ target, Engine::Vector3 offset)
{
	Ogre::Vector3 ogOffset(offset.x, offset.y, offset.z);
	sceneNode->setAutoTracking(enabled, target->getSceneNode(), ogOffset);
}

void SceneNode::setVisible(bool visible)
{
	sceneNode->setVisible(visible);
}

void SceneNode::setVisible(bool visible, bool cascade)
{
	sceneNode->setVisible(visible, cascade);
}

SceneNode^ SceneNode::getManagedNode(Ogre::SceneNode* node)
{
	SceneNodeGCRoot* root = static_cast<SceneNodeGCRoot*>(Ogre::any_cast<void*>(node->getUserAny()));
	return (*root);
}

}