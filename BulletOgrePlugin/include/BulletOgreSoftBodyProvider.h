#pragma once

using namespace BulletPlugin;
using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace OgrePlugin;
using namespace OgreWrapper;
using namespace System;

class btSoftBody;

namespace BulletOgrePlugin
{

ref class BulletOgreSoftBodyProviderDefinition;

ref class BulletOgreSoftBodyProvider : public BulletPlugin::SoftBodyProvider
{
private:
	btSoftBody* softBody;
	BulletScene^ scene;
	SceneNode^ node;
	Entity^ entity;
	OgreSceneManager^ ogreScene;
	String^ meshName;
	String^ groupName;

	int *mDupVertices;
	int *mNewIndexes;
	int mDupVerticesCount;

protected:
	virtual void updatePositionImpl(Vector3% translation, Quaternion% rotation) override;

	virtual void updateTranslationImpl(Vector3% translation) override;

	virtual void updateRotationImpl(Quaternion% rotation) override;

	virtual void updateScaleImpl(Vector3% scale) override;

	virtual void setEnabled(bool enabled) override;

	virtual void* createSoftBodyImpl(BulletPlugin::BulletScene^ scene) override;

	virtual void destroySoftBodyImpl(BulletScene^ scene) override;

	virtual void createStaticRepresentationImpl() override;

	virtual void destroyStaticRepresentationImpl() override;

	int getBulletIndex(int idx);
public:
	BulletOgreSoftBodyProvider(BulletOgreSoftBodyProviderDefinition^ def, OgreSceneManager^ ogreScene);

	virtual ~BulletOgreSoftBodyProvider(void);

	virtual SimElementDefinition^ saveToDefinition() override;

	virtual void updateOtherSubsystems() override;
};

}