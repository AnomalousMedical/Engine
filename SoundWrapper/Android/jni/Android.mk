LOCAL_PATH := $(call my-dir)

DEP_ROOT		:= $(LOCAL_PATH)/../../../../Dependencies
DEP_ROOT_LIB	:= ../../../../Dependencies

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \
				   $(DEP_ROOT)/OpenALSoft/srcAndroid/jni/OpenAL \
				   $(DEP_ROOT)/oggvorbis/libogg/include \
				   $(DEP_ROOT)/oggvorbis/libvorbis/include \

PRJ_SRC		    := ../../Src

OPENAL_LIB_ROOT		:= $(DEP_ROOT_LIB)/OpenALSoft/srcAndroid/libs/$(TARGET_ARCH_ABI)
OGG_LIB_ROOT		:= $(DEP_ROOT_LIB)/oggvorbis/libogg/android/obj/local/$(TARGET_ARCH_ABI)
VORBIS_LIB_ROOT		:= $(DEP_ROOT_LIB)/oggvorbis/libvorbis/android/obj/local/$(TARGET_ARCH_ABI)

# openalsoft
	include $(CLEAR_VARS)
	LOCAL_MODULE    := openalsoft
	LOCAL_SRC_FILES := $(OPENAL_LIB_ROOT)/libopenal.so
	include $(PREBUILT_SHARED_LIBRARY)

# libogg
	include $(CLEAR_VARS)
	LOCAL_MODULE    := libogg
	LOCAL_SRC_FILES := $(OGG_LIB_ROOT)/libogg.a
	include $(PREBUILT_STATIC_LIBRARY)

# libvorbis
	include $(CLEAR_VARS)
	LOCAL_MODULE    := libvorbis
	LOCAL_SRC_FILES := $(VORBIS_LIB_ROOT)/libvorbis.a
	include $(PREBUILT_STATIC_LIBRARY)

#Main Project

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE   := libSoundWrapper
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -D_ARM_ASSEM_ \
				  -frtti -fexceptions

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES :=  \
                    $(PRJ_SRC)/../Stdafx.cpp \
                    $(PRJ_SRC)/AudioCodec.cpp \
                    $(PRJ_SRC)/CaptureDevice.cpp \
                    $(PRJ_SRC)/Listener.cpp \
                    $(PRJ_SRC)/ManagedLogListener.cpp \
                    $(PRJ_SRC)/ManagedStream.cpp \
                    $(PRJ_SRC)/MemorySound.cpp \
                    $(PRJ_SRC)/NativeLog.cpp \
                    $(PRJ_SRC)/NativeStream.cpp \
                    $(PRJ_SRC)/OggCodec.cpp \
                    $(PRJ_SRC)/OggEncoder.cpp \
                    $(PRJ_SRC)/OpenALManager.cpp \
                    $(PRJ_SRC)/Sound.cpp \
                    $(PRJ_SRC)/Source.cpp \
                    $(PRJ_SRC)/SourceManager.cpp \
                    $(PRJ_SRC)/StreamingSound.cpp \


LOCAL_SHARED_LIBRARIES	:= openalsoft
LOCAL_STATIC_LIBRARIES	:= libvorbis libogg

include $(BUILD_SHARED_LIBRARY)
