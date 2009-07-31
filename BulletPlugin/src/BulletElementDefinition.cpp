#include "StdAfx.h"
#include "..\include\BulletElementDefinition.h"

namespace BulletPlugin
{

BulletElementDefinition::BulletElementDefinition(String^ name)
:SimElementDefinition(name)
{
}

BulletElementDefinition::BulletElementDefinition(LoadInfo^ info)
:SimElementDefinition(info)
{

}

void BulletElementDefinition::getInfo(SaveInfo^ info)
{
	SimElementDefinition::getInfo(info);
}

}