LOCAL_PATH := $(call my-dir)

DEP_ROOT		:= $(LOCAL_PATH)/../../../../Dependencies
DEP_ROOT_LIB	:= ../../../../Dependencies
ENGINE_ROOT     := ../../..

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \
				   $(DEP_ROOT)/libRocket/src/Include \
	               $(DEP_ROOT)/Ogre/src/OgreMain/include \
				   $(DEP_ROOT)/Ogre/AndroidBuild/include \
				   $(DEP_ROOT)/OgreDeps/AndroidInstall/include \

PRJ_SRC		    := ../../Src

ROCKET_LIB_ROOT		:= $(DEP_ROOT_LIB)/libRocket/AndroidBuild/libs/$(TARGET_ARCH_ABI)
OGRE_LIB_ROOT		:= $(ENGINE_ROOT)/OgreCWrapper/Android/libs/armeabi-v7a
OGREDEP_LIB_ROOT	:= $(DEP_ROOT_LIB)/OgreDeps/AndroidInstall/lib

# ogremain
	include $(CLEAR_VARS)
	LOCAL_MODULE    := ogremain
	LOCAL_SRC_FILES := $(OGRE_LIB_ROOT)/libOgreCWrapper.so
	include $(PREBUILT_SHARED_LIBRARY)

# librocketcontrols
	include $(CLEAR_VARS)
	LOCAL_MODULE    := librocketcontrols
	LOCAL_SRC_FILES := $(ROCKET_LIB_ROOT)/libRocketControls.a
	include $(PREBUILT_STATIC_LIBRARY)

# librocketcore
	include $(CLEAR_VARS)
	LOCAL_MODULE    := librocketcore
	LOCAL_SRC_FILES := $(ROCKET_LIB_ROOT)/libRocketCore.a
	include $(PREBUILT_STATIC_LIBRARY)

# librocketdebugger
	include $(CLEAR_VARS)
	LOCAL_MODULE    := librocketdebugger
	LOCAL_SRC_FILES := $(ROCKET_LIB_ROOT)/libRocketDebugger.a
	include $(PREBUILT_STATIC_LIBRARY)

# freetype
	include $(CLEAR_VARS)
	LOCAL_MODULE    := freetype
	LOCAL_SRC_FILES := $(OGREDEP_LIB_ROOT)/libfreetype.a
	include $(PREBUILT_STATIC_LIBRARY)

#Main Project

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE   := libRocketWrapper
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -D_ARM_ASSEM_ \
				  -frtti -fexceptions

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES :=  \
                    $(PRJ_SRC)/../Stdafx.cpp \
                    $(PRJ_SRC)/Context.cpp \
                    $(PRJ_SRC)/Controls.cpp \
                    $(PRJ_SRC)/Core.cpp \
                    $(PRJ_SRC)/Debugger.cpp \
                    $(PRJ_SRC)/Dictionary.cpp \
                    $(PRJ_SRC)/Element.cpp \
                    $(PRJ_SRC)/ElementDocument.cpp \
                    $(PRJ_SRC)/ElementFormControl.cpp \
                    $(PRJ_SRC)/ElementListIter.cpp \
                    $(PRJ_SRC)/ElementManager.cpp \
                    $(PRJ_SRC)/Event.cpp \
                    $(PRJ_SRC)/Factory.cpp \
                    $(PRJ_SRC)/FontDatabase.cpp \
                    $(PRJ_SRC)/ManagedEventInstancer.cpp \
                    $(PRJ_SRC)/ManagedEventListener.cpp \
                    $(PRJ_SRC)/ManagedEventListenerInstancer.cpp \
                    $(PRJ_SRC)/ManagedFileInterface.cpp \
                    $(PRJ_SRC)/ManagedSystemInterface.cpp \
                    $(PRJ_SRC)/ReferenceCountable.cpp \
                    $(PRJ_SRC)/RenderInterfaceOgre3D.cpp \
                    $(PRJ_SRC)/Template.cpp \
                    $(PRJ_SRC)/TemplateCache.cpp \
                    $(PRJ_SRC)/TextureDatabase.cpp \
                    $(PRJ_SRC)/Variant.cpp \



LOCAL_SHARED_LIBRARIES	:= ogremain
LOCAL_STATIC_LIBRARIES	:= librocketdebugger librocketcontrols librocketcore freetype

include $(BUILD_SHARED_LIBRARY)
