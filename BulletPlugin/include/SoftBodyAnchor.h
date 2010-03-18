#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;

namespace BulletPlugin
{

ref class SoftBodyAnchorDefinition;

/// <summary>
/// This class is a placeholder inside of a SimObject for soft body anchors. 
/// These do not have any manipulation that can be done so this class basicly does nothing.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
ref class SoftBodyAnchor : public SimElement
{
private:
	String^ rigidBodySimObject;
	String^ rigidBodyElement;
	String^ softBodySimObject;
	String^ softBodyElement;
	bool disableCollisionBetweenNodes;
	bool findClosestNode;
	int specificNode;

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

public:
	SoftBodyAnchor(SoftBodyAnchorDefinition^ definition);

	virtual ~SoftBodyAnchor(void);

	virtual SimElementDefinition^ saveToDefinition() override;
};

}