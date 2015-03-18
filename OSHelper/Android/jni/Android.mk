LOCAL_PATH := $(call my-dir)

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \

PRJ_SRC		    := /../../Src

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE    := libOSHelper
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -DBYTE_ORDER=LITTLE_ENDIAN -D_ARM_ASSEM_

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES := \
					$(PRJ_SRC)/../Stdafx.cpp \
					$(PRJ_SRC)/NativeLog.cpp \
					$(PRJ_SRC)/ManagedLogListener.cpp \
					$(PRJ_SRC)/App.cpp \
					$(PRJ_SRC)/NativeKeyboard.cpp \
					$(PRJ_SRC)/NativeMouse.cpp \
					$(PRJ_SRC)/NativeOSWindow.cpp \
					$(PRJ_SRC)/PerformanceCounter.cpp \
					$(PRJ_SRC)/AndroidApp.cpp \

LOCAL_LDLIBS    := -llog -landroid
LOCAL_STATIC_LIBRARIES := android_native_app_glue

include $(BUILD_SHARED_LIBRARY)

$(call import-module,android/native_app_glue)
