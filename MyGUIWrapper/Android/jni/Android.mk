LOCAL_PATH := $(call my-dir)

DEP_ROOT		:= $(LOCAL_PATH)/../../../../Dependencies
DEP_ROOT_LIB	:= ../../../../Dependencies
ENGINE_ROOT     := ../../..

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \
				   $(DEP_ROOT)/MyGUI/Src/MyGUIEngine/include \
				   $(DEP_ROOT)/MyGUI/Src/Platforms/Ogre/OgrePlatform/include \
				   $(DEP_ROOT)/MyGUI/Src/Common \
	               $(DEP_ROOT)/Ogre/src/OgreMain/include \
				   $(DEP_ROOT)/Ogre/AndroidBuild/include \
				   $(DEP_ROOT)/OgreDeps/AndroidInstall/include \

PRJ_SRC		    := /../../Src

MYGUI_LIB_ROOT		:= $(DEP_ROOT_LIB)/MyGUI/AndroidBuild/lib
OGRE_LIB_ROOT		:= $(ENGINE_ROOT)/OgreCWrapper/Android/libs/armeabi-v7a
OGREDEP_LIB_ROOT	:= $(DEP_ROOT_LIB)/OgreDeps/AndroidInstall/lib

# ogremain
	include $(CLEAR_VARS)
	LOCAL_MODULE    := ogremain
	LOCAL_SRC_FILES := $(OGRE_LIB_ROOT)/libOgreCWrapper.so
	include $(PREBUILT_SHARED_LIBRARY)

# freetype
	include $(CLEAR_VARS)
	LOCAL_MODULE    := freetype
	LOCAL_SRC_FILES := $(OGREDEP_LIB_ROOT)/libfreetype.a
	include $(PREBUILT_STATIC_LIBRARY)

# myguiengine
	include $(CLEAR_VARS)
	LOCAL_MODULE    := myguiengine
	LOCAL_SRC_FILES := $(MYGUI_LIB_ROOT)/libMyGUIEngineStatic.a
	include $(PREBUILT_STATIC_LIBRARY)

# ogreplatform
	include $(CLEAR_VARS)
	LOCAL_MODULE    := ogreplatform
	LOCAL_SRC_FILES := $(MYGUI_LIB_ROOT)/libMyGUI.OgrePlatform.a
	include $(PREBUILT_STATIC_LIBRARY)

#Main Project

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE   := libMyGUIWrapper
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -D_ARM_ASSEM_ \
				  -frtti -fexceptions

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES :=  \
                    $(PRJ_SRC)/../Stdafx.cpp \
                    $(PRJ_SRC)/Button.cpp \
                    $(PRJ_SRC)/ClickEventTranslator.cpp \
                    $(PRJ_SRC)/ComboBox.cpp \
                    $(PRJ_SRC)/EditBox.cpp \
                    $(PRJ_SRC)/EventCanvasPositionChanged.cpp \
                    $(PRJ_SRC)/EventChangeCoord.cpp \
                    $(PRJ_SRC)/EventChangeKeyFocusInputManager.cpp \
                    $(PRJ_SRC)/EventChangeMouseFocusInputManager.cpp \
                    $(PRJ_SRC)/EventChangeMousePointerTranslator.cpp \
                    $(PRJ_SRC)/EventComboAcceptTranslator.cpp \
                    $(PRJ_SRC)/EventComboChangePositionTranslator.cpp \
                    $(PRJ_SRC)/EventEditSelectAcceptTranslator.cpp \
                    $(PRJ_SRC)/EventEditTextChangeTranslator.cpp \
                    $(PRJ_SRC)/EventKeyButtonPressedTranslator.cpp \
                    $(PRJ_SRC)/EventKeyButtonReleasedTranslator.cpp \
                    $(PRJ_SRC)/EventKeyLostFocusTranslator.cpp \
                    $(PRJ_SRC)/EventKeySetFocusTranslator.cpp \
                    $(PRJ_SRC)/EventListChangePositionTranslator.cpp \
                    $(PRJ_SRC)/EventListSelectAcceptTranslator.cpp \
                    $(PRJ_SRC)/EventMenuCtrlAcceptTranslator.cpp \
                    $(PRJ_SRC)/EventMenuCtrlCloseTranslator.cpp \
                    $(PRJ_SRC)/EventMessageBoxResultTranslator.cpp \
                    $(PRJ_SRC)/EventMouseButtonDoubleClickTranslator.cpp \
                    $(PRJ_SRC)/EventMouseButtonPressedTranslator.cpp \
                    $(PRJ_SRC)/EventMouseButtonReleasedTranslator.cpp \
                    $(PRJ_SRC)/EventMouseDragTranslator.cpp \
                    $(PRJ_SRC)/EventMouseLostFocusTranslator.cpp \
                    $(PRJ_SRC)/EventMouseMoveTranslator.cpp \
                    $(PRJ_SRC)/EventMouseSetFocusTranslator.cpp \
                    $(PRJ_SRC)/EventMouseWheelTranslator.cpp \
                    $(PRJ_SRC)/EventRootKeyChangeFocusTranslator.cpp \
                    $(PRJ_SRC)/EventRootMouseChangeFocusTranslator.cpp \
                    $(PRJ_SRC)/EventScrollGesture.cpp \
                    $(PRJ_SRC)/EventToolTipTranslator.cpp \
                    $(PRJ_SRC)/EventWindowButtonPressedTranslator.cpp \
                    $(PRJ_SRC)/EventWindowChangeCoordTranslator.cpp \
                    $(PRJ_SRC)/FontManager.cpp \
                    $(PRJ_SRC)/Gui.cpp \
                    $(PRJ_SRC)/ImageBox.cpp \
                    $(PRJ_SRC)/InputManager.cpp \
                    $(PRJ_SRC)/ISubWidgetText.cpp \
                    $(PRJ_SRC)/LanguageManager.cpp \
                    $(PRJ_SRC)/LayerManager.cpp \
                    $(PRJ_SRC)/Layout.cpp \
                    $(PRJ_SRC)/LayoutManager.cpp \
                    $(PRJ_SRC)/ManagedMyGUILogListener.cpp \
                    $(PRJ_SRC)/MenuControl.cpp \
                    $(PRJ_SRC)/MenuItem.cpp \
                    $(PRJ_SRC)/Message.cpp \
                    $(PRJ_SRC)/MultiListBox.cpp \
                    $(PRJ_SRC)/MyGUIEventTranslator.cpp \
                    $(PRJ_SRC)/OgrePlatform.cpp \
                    $(PRJ_SRC)/OgreRenderManager.cpp \
                    $(PRJ_SRC)/PointerManager.cpp \
                    $(PRJ_SRC)/ProgressBar.cpp \
                    $(PRJ_SRC)/RenderManager.cpp \
                    $(PRJ_SRC)/ResourceManager.cpp \
                    $(PRJ_SRC)/ScrollBar.cpp \
                    $(PRJ_SRC)/ScrollChangePositionET.cpp \
                    $(PRJ_SRC)/ScrollView.cpp \
                    $(PRJ_SRC)/TabControl.cpp \
                    $(PRJ_SRC)/TextBox.cpp \
                    $(PRJ_SRC)/Widget.cpp \
                    $(PRJ_SRC)/WidgetManager.cpp \
                    $(PRJ_SRC)/Window.cpp \


LOCAL_SHARED_LIBRARIES	:= ogremain
LOCAL_STATIC_LIBRARIES	:= ogreplatform myguiengine freetype

include $(BUILD_SHARED_LIBRARY)
