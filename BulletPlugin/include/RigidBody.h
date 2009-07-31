#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;

namespace BulletPlugin
{

class MotionState;
ref class RigidBodyDefinition;
ref class BulletScene;

public ref class RigidBody : public SimElement
{
private:
	MotionState* motionState;
	BulletScene^ scene;
	btRigidBody* rigidBody;

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

public:
	RigidBody(RigidBodyDefinition^ description, BulletScene^ scene);

	virtual ~RigidBody(void);

	virtual SimElementDefinition^ saveToDefinition() override;

	void setWorldTransform(Vector3 translation, Quaternion rotation);
};

}