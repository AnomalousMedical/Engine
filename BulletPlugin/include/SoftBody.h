#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;

class btSoftBody;

namespace BulletPlugin
{

ref class SoftBodyDefinition;
ref class BulletScene;
ref class SoftBodyProvider;

[Engine::Attributes::NativeSubsystemType]
public ref class SoftBody : public SimElement
{
private:
	BulletScene^ scene;
	btSoftBody* softBody;
	float mass; //recorded because the getTotalMass function looses some value each time it is called.
	bool massFromFaces;
	bool randomizeConstraints;
	SoftBodyProvider^ sbProvider;
	bool generateBendingConstraints;
	int bendingConstraintDistance;

	//Pose
	bool setPose;
	bool setPoseVolume;
	bool setPoseFrame;

internal:
	void setInitialPosition(Vector3% translation, Quaternion% rotation)
	{
		updatePositionImpl(translation, rotation);
	}

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

public:
	SoftBody(SoftBodyDefinition^ description, BulletScene^ scene, SoftBodyProvider^ sbProvider);

	virtual ~SoftBody(void);

	virtual SimElementDefinition^ saveToDefinition() override;
};

}