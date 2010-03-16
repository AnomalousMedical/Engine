#pragma once

#include "SoftBodyProvider.h"

class btSoftBody;

namespace BulletPlugin
{

ref class EllipsoidSoftBodyProviderDefinition;

ref class EllipsoidSoftBodyProvider : public SoftBodyProvider
{
private:
	btSoftBody* softBody;
	BulletScene^ scene;

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

	virtual void* createSoftBodyImpl(BulletScene^ scene) override;

	virtual void destroySoftBodyImpl(BulletScene^ scene) override;

	virtual void createStaticRepresentationImpl() override;

	virtual void destroyStaticRepresentationImpl() override;

public:
	EllipsoidSoftBodyProvider(EllipsoidSoftBodyProviderDefinition^ def);

	virtual ~EllipsoidSoftBodyProvider(void);

	virtual SimElementDefinition^ saveToDefinition() override;

	virtual void updateOtherSubsystems() override;
};

}