#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;

namespace BulletPlugin
{

ref class BulletFactory : SimElementFactory
{
public:
	BulletFactory(void);

	virtual void createProducts();

	virtual void createStaticProducts();

	virtual void linkProducts();

	virtual void clearDefinitions();
};

}