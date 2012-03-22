#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::LayerManager* LayerManager_getSingletonPtr()
{
	return MyGUI::LayerManager::getInstancePtr();
}

extern "C" _AnomalousExport void LayerManager_attachToLayerNode(MyGUI::LayerManager* layerManager, String name, MyGUI::Widget* item)
{
	layerManager->attachToLayerNode(name, item);
}

extern "C" _AnomalousExport void LayerManager_detachFromLayer(MyGUI::LayerManager* layerManager, MyGUI::Widget* item)
{
	layerManager->detachFromLayer(item);
}

extern "C" _AnomalousExport void LayerManager_upLayerItem(MyGUI::LayerManager* layerManager, MyGUI::Widget* item)
{
	layerManager->upLayerItem(item);
}

extern "C" _AnomalousExport bool LayerManager_isExist(MyGUI::LayerManager* layerManager, String name)
{
	return layerManager->isExist(name);
}

extern "C" _AnomalousExport MyGUI::Widget* LayerManager_getWidgetFromPoint(MyGUI::LayerManager* layerManager, int left, int top)
{
	return layerManager->getWidgetFromPoint(left, top);
}