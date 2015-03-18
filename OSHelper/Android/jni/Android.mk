LOCAL_PATH := $(call my-dir)

PRJ_SRC		    := /../../Src

include $(CLEAR_VARS)

LOCAL_MODULE    := libOSHelper
LOCAL_LDLIBS    := -llog -landroid -lEGL -lGLESv1_CM
LOCAL_STATIC_LIBRARIES := android_native_app_glue

LOCAL_SRC_FILES := \
					$(PRJ_SRC)/AndroidApp.cpp \

include $(BUILD_SHARED_LIBRARY)

$(call import-module,android/native_app_glue)
