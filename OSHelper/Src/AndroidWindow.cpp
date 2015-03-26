#include "StdAfx.h"
#include "AndroidWindow.h"
#include "AndroidApp.h"

static struct android_app* app = 0;
static float _screenDensity = 1.0f;
NativeAction _toggleKeyboard;

void displayKeyboard(bool pShow);

AndroidWindow::AndroidWindow()
	:keyboardVisible(false)
{
	struct AndroidAppState* appState = (struct AndroidAppState*)app->userData;
	appState->androidWindow = this;
}

AndroidWindow::~AndroidWindow()
{
	
}

void AndroidWindow::setTitle(String title)
{
	
}

void AndroidWindow::setSize(int width, int height)
{
	
}

int AndroidWindow::getWidth()
{
	return ANativeWindow_getWidth(app->window);
}

int AndroidWindow::getHeight()
{
	return ANativeWindow_getHeight(app->window);
}

void* AndroidWindow::getHandle()
{
	//On android we return the pointer to our app instead of the window, this allows more flexibility in
	//external libs.
	return (void*)app;
}

void AndroidWindow::show()
{
	
}

void AndroidWindow::toggleFullscreen()
{
	
}

void AndroidWindow::close()
{
	
}

void AndroidWindow::setMaximized(bool maximized)
{
	
}

bool AndroidWindow::getMaximized()
{
	return true;
}

void AndroidWindow::setCursor(CursorType cursor)
{
	
}

float AndroidWindow::getWindowScaling()
{
	return _screenDensity;
}

void AndroidWindow::setOnscreenKeyboardVisible(bool visible)
{
	//LOGI("Requesting keyboard visible %i", visible);
	//displayKeyboard(visible);
	_toggleKeyboard();
	keyboardVisible = visible;
}

bool AndroidWindow::isOnscreenKeyboardVisible()
{
	return keyboardVisible;
}

int32_t AndroidWindow::handleInputEvent(struct android_app* app, AInputEvent* event)
{
	if (AInputEvent_getType(event) == AINPUT_EVENT_TYPE_MOTION)
	{
		//int deviceId = AInputEvent_getDeviceId(event); //We can use this to identify if we are a mouse or the touchscreen or something else

		eventAction = AMotionEvent_getAction(event);
		action = (int)(AMOTION_EVENT_ACTION_MASK & eventAction);

		switch (action)
		{
		case AMOTION_EVENT_ACTION_DOWN:
			touchInfo.id = AMotionEvent_getPointerId(event, 0);
			touchInfo.pixelX = AMotionEvent_getRawX(event, 0);
			touchInfo.pixelY = AMotionEvent_getRawY(event, 0);
			multiTouch->fireTouchStarted(touchInfo);
			//LOGI("Motion event down id: %i x: %i y: %i", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
			break;

		case AMOTION_EVENT_ACTION_UP:
			touchInfo.id = AMotionEvent_getPointerId(event, 0);
			touchInfo.pixelX = AMotionEvent_getRawX(event, 0);
			touchInfo.pixelY = AMotionEvent_getRawY(event, 0);
			multiTouch->fireTouchEnded(touchInfo);
			//LOGI("Motion event up id: %i x: %i y: %i", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
			break;

		case AMOTION_EVENT_ACTION_MOVE:
			pointerCount = AMotionEvent_getPointerCount(event);
			for (int i = 0; i < pointerCount; ++i)
			{
				touchInfo.id = AMotionEvent_getPointerId(event, i);
				touchInfo.pixelX = AMotionEvent_getRawX(event, i);
				touchInfo.pixelY = AMotionEvent_getRawY(event, i);
				multiTouch->fireTouchMoved(touchInfo);
				//LOGI("Motion event move id: %i x: %i y: %i", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
			}
			break;

		case AMOTION_EVENT_ACTION_CANCEL:
			multiTouch->fireAllTouchesCanceled();
			//LOGI("Motion event cancel");
			break;

		case AMOTION_EVENT_ACTION_POINTER_DOWN:
			eventPointerIndex = (int)(AMOTION_EVENT_ACTION_POINTER_INDEX_MASK & eventAction) >> AMOTION_EVENT_ACTION_POINTER_INDEX_SHIFT;
			touchInfo.id = AMotionEvent_getPointerId(event, eventPointerIndex);
			touchInfo.pixelX = AMotionEvent_getRawX(event, eventPointerIndex);
			touchInfo.pixelY = AMotionEvent_getRawY(event, eventPointerIndex);
			multiTouch->fireTouchStarted(touchInfo);
			//LOGI("Motion event pointer down id: %i x: %i y: %i", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
			break;

		case AMOTION_EVENT_ACTION_POINTER_UP:
			eventPointerIndex = (int)(AMOTION_EVENT_ACTION_POINTER_INDEX_MASK & eventAction) >> AMOTION_EVENT_ACTION_POINTER_INDEX_SHIFT;
			touchInfo.id = AMotionEvent_getPointerId(event, eventPointerIndex);
			touchInfo.pixelX = AMotionEvent_getRawX(event, eventPointerIndex);
			touchInfo.pixelY = AMotionEvent_getRawY(event, eventPointerIndex);
			multiTouch->fireTouchEnded(touchInfo);
			//LOGI("Motion event pointer up id: %i x: %i y: %i", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
			break;

		//The following are more mouse events, will try to handle these also somehow
		//case AMOTION_EVENT_ACTION_HOVER_MOVE:
		//	touchInfo.id = AMotionEvent_getPointerId(event, 0);
		//	touchInfo.pixelX = AMotionEvent_getRawX(event, 0);
		//	touchInfo.pixelY = AMotionEvent_getRawY(event, 0);
		//	LOGI("Motion event hover move id: %i x: %f y: %f", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
		//	break;

		//case AMOTION_EVENT_ACTION_SCROLL:
		//	touchInfo.id = AMotionEvent_getPointerId(event, 0);
		//	LOGI("Motion event scroll id: %i x: %f y: %f", touchInfo.id, touchInfo.pixelX, touchInfo.pixelY);
		//	break;
		}

		return 1;
	}
	return 0;
}

void AndroidWindow_setApp(struct android_app* setApp)
{
	app = setApp;
}

//PInvoke
extern "C" _AnomalousExport NativeOSWindow* NativeOSWindow_create(NativeOSWindow* parent, String caption, int x, int y, int width, int height, bool floatOnParent)
{
	return new AndroidWindow();
}

//This function is used to set some attributes that would otherwise require a bunch of jni calls, we can get them really easily
//on the c# side, however.
extern "C" _AnomalousExport void AndroidOSWindow_EasyAttributeSetup(float screenDensity, NativeAction toggleKeyboard)
{
	_screenDensity = screenDensity;
	_toggleKeyboard = toggleKeyboard;
}