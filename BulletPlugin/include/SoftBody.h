#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;

class btSoftBody;

namespace BulletPlugin
{

ref class SoftBodyDefinition;
ref class BulletScene;

[Engine::Attributes::NativeSubsystemType]
public ref class SoftBody : public SimElement
{
private:
	BulletScene^ scene;
	btSoftBody* softBody;
	float mass; //recorded because the getTotalMass function looses some value each time it is called.
	bool massFromFaces;
	bool randomizeConstraints;

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

public:
	SoftBody(SoftBodyDefinition^ description, BulletScene^ scene);

	virtual ~SoftBody(void);

	virtual SimElementDefinition^ saveToDefinition() override;
};

}