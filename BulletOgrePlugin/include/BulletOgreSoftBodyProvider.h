#pragma once

using namespace BulletPlugin;
using namespace Engine;
using namespace Engine::ObjectManagement;

class btSoftBody;

namespace BulletOgrePlugin
{

ref class BulletOgreSoftBodyProviderDefinition;

ref class BulletOgreSoftBodyProvider : public BulletPlugin::SoftBodyProvider
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

	virtual void* createSoftBodyImpl(BulletPlugin::BulletScene^ scene) override;

	virtual void destroySoftBodyImpl(BulletScene^ scene) override;

public:
	BulletOgreSoftBodyProvider(BulletOgreSoftBodyProviderDefinition^ def);

	virtual ~BulletOgreSoftBodyProvider(void);

	virtual SimElementDefinition^ saveToDefinition() override;

	virtual void updateOtherSubsystems() override;
};

}