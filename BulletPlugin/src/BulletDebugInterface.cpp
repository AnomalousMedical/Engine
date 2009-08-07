#include "StdAfx.h"
#include "..\include\BulletDebugInterface.h"
#include "BulletDebugEntry.h"
#include "BulletScene.h"

namespace BulletPlugin
{

BulletDebugInterface::BulletDebugInterface(void)
:enabled(false),
debugEntries(gcnew List<DebugEntry^>())
{
	debugEntries->Add(gcnew BulletDebugEntry("Draw Wireframe", btIDebugDraw::DBG_DrawWireframe));
	debugEntries->Add(gcnew BulletDebugEntry("Draw AABB", btIDebugDraw::DBG_DrawAabb));
	debugEntries->Add(gcnew BulletDebugEntry("Draw Features Text", btIDebugDraw::DBG_DrawFeaturesText));
	debugEntries->Add(gcnew BulletDebugEntry("Draw Contact Points", btIDebugDraw::DBG_DrawContactPoints));
	debugEntries->Add(gcnew BulletDebugEntry("No Deactivation", btIDebugDraw::DBG_NoDeactivation));
	debugEntries->Add(gcnew BulletDebugEntry("No Help Text", btIDebugDraw::DBG_NoHelpText));
	debugEntries->Add(gcnew BulletDebugEntry("Draw Text", btIDebugDraw::DBG_DrawText));
	debugEntries->Add(gcnew BulletDebugEntry("Profile Timings", btIDebugDraw::DBG_ProfileTimings));
	debugEntries->Add(gcnew BulletDebugEntry("Enable Sat Comparison", btIDebugDraw::DBG_EnableSatComparison));
	debugEntries->Add(gcnew BulletDebugEntry("Disable Bullet LCP", btIDebugDraw::DBG_DisableBulletLCP));
	debugEntries->Add(gcnew BulletDebugEntry("Enable CCD", btIDebugDraw::DBG_EnableCCD));
	debugEntries->Add(gcnew BulletDebugEntry("Draw Constraints", btIDebugDraw::DBG_DrawConstraints));
	debugEntries->Add(gcnew BulletDebugEntry("Draw Constraint Limits", btIDebugDraw::DBG_DrawConstraintLimits));
	//debugEntries->Add(gcnew BulletDebugEntry("Fast Wireframe", btIDebugDraw::DBG_FastWireframe));
}

BulletDebugInterface::~BulletDebugInterface(void)
{

}

IEnumerable<DebugEntry^>^ BulletDebugInterface::getEntries()
{
	return debugEntries;
}

void BulletDebugInterface::renderDebug(DebugDrawingSurface^ drawingSurface, SimSubScene^ subScene)
{
	if(enabled)
	{
		BulletScene^ sceneManager = subScene->getSimElementManager<BulletScene^>();
		if(sceneManager != nullptr)
		{
			sceneManager->drawDebug(drawingSurface);
		}
	}
}

void BulletDebugInterface::setEnabled(bool enabled)
{
	this->enabled = enabled;
}

}