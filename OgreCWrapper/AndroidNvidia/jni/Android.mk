LOCAL_PATH := $(call my-dir)

DEP_ROOT		:= $(LOCAL_PATH)/../../../../Dependencies
DEP_ROOT_LIB	:= ../../../../Dependencies

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \
	               $(DEP_ROOT)/Ogre/src/OgreMain/include \
				   $(DEP_ROOT)/Ogre/src/RenderSystems/GLES2/include \
				   $(DEP_ROOT)/Ogre/src/RenderSystems/GLES2/include/EGL \
				   $(DEP_ROOT)/Ogre/AndroidBuild/include \
				   $(DEP_ROOT)/OgreDeps/AndroidInstall/include \

PRJ_SRC		    := ../../Src

OGRE_LIB_ROOT		:= $(DEP_ROOT_LIB)/Ogre/AndroidBuild/lib
OGREDEP_LIB_ROOT	:= $(DEP_ROOT_LIB)/OgreDeps/AndroidInstall/lib

# ogremain
	include $(CLEAR_VARS)
	LOCAL_MODULE    := ogremain
	LOCAL_SRC_FILES := $(OGRE_LIB_ROOT)/libOgreMainStatic.a
	include $(PREBUILT_STATIC_LIBRARY)

# rendersystem_gles2
	include $(CLEAR_VARS)
	LOCAL_MODULE    := rendersystem_gles2
	LOCAL_SRC_FILES := $(OGRE_LIB_ROOT)/libRenderSystem_GLES2Static.a
	include $(PREBUILT_STATIC_LIBRARY)

# freetype
	include $(CLEAR_VARS)
	LOCAL_MODULE    := freetype
	LOCAL_SRC_FILES := $(OGREDEP_LIB_ROOT)/libfreetype.a
	include $(PREBUILT_STATIC_LIBRARY)

# FreeImage
	include $(CLEAR_VARS)
	LOCAL_MODULE    := FreeImage
	LOCAL_SRC_FILES := $(OGREDEP_LIB_ROOT)/libFreeImage.a
	include $(PREBUILT_STATIC_LIBRARY)

# zlib
	include $(CLEAR_VARS)
	LOCAL_MODULE    := zlib
	LOCAL_SRC_FILES := $(OGREDEP_LIB_ROOT)/libzlib.a
	include $(PREBUILT_STATIC_LIBRARY)

# zzip
	include $(CLEAR_VARS)
	LOCAL_MODULE    := zzip
	LOCAL_SRC_FILES := $(OGREDEP_LIB_ROOT)/libzziplib.a
	include $(PREBUILT_STATIC_LIBRARY)

# libTegra_gfx_debugger
	include $(CLEAR_VARS)
	LOCAL_MODULE    := libTegra_gfx_debugger
	LOCAL_SRC_FILES := $(LOCAL_PATH)/../../libIn/libTegra_gfx_debugger.a
	include $(PREBUILT_STATIC_LIBRARY)

	

#Main Project

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE   := libOgreCWrapper
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -D_ARM_ASSEM_ \
				  -frtti -fexceptions

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES :=  \
                    $(PRJ_SRC)/../Stdafx.cpp \
                    $(PRJ_SRC)/Animation.cpp \
                    $(PRJ_SRC)/AnimationState.cpp \
                    $(PRJ_SRC)/AnimationStateIterator.cpp \
                    $(PRJ_SRC)/AnimationStateSet.cpp \
                    $(PRJ_SRC)/AnimationTrack.cpp \
                    $(PRJ_SRC)/ArchiveManager.cpp \
                    $(PRJ_SRC)/Bone.cpp \
                    $(PRJ_SRC)/Camera.cpp \
                    $(PRJ_SRC)/CompositorManager.cpp \
                    $(PRJ_SRC)/Entity.cpp \
                    $(PRJ_SRC)/GpuProgramManager.cpp \
                    $(PRJ_SRC)/HardwareBuffer.cpp \
                    $(PRJ_SRC)/HardwareBufferManager.cpp \
                    $(PRJ_SRC)/HardwareIndexBuffer.cpp \
                    $(PRJ_SRC)/HardwarePixelBuffer.cpp \
                    $(PRJ_SRC)/HardwareVertexBuffer.cpp \
                    $(PRJ_SRC)/IndexData.cpp \
                    $(PRJ_SRC)/KeyFrame.cpp \
                    $(PRJ_SRC)/Light.cpp \
					$(PRJ_SRC)/ManagedManualResourceLoader.cpp \
                    $(PRJ_SRC)/ManagedRenderQueueListener.cpp \
                    $(PRJ_SRC)/ManagedRenderSystemListener.cpp \
                    $(PRJ_SRC)/ManagedRenderTargetListener.cpp \
                    $(PRJ_SRC)/ManualObject.cpp \
                    $(PRJ_SRC)/ManualObjectSection.cpp \
                    $(PRJ_SRC)/Material.cpp \
                    $(PRJ_SRC)/MaterialManager.cpp \
                    $(PRJ_SRC)/Mesh.cpp \
                    $(PRJ_SRC)/MeshManager.cpp \
                    $(PRJ_SRC)/MeshSerializer.cpp \
                    $(PRJ_SRC)/MovableObject.cpp \
                    $(PRJ_SRC)/NativeFrameListener.cpp \
                    $(PRJ_SRC)/NativeSceneListener.cpp \
                    $(PRJ_SRC)/NativeWindowListener.cpp \
                    $(PRJ_SRC)/Node.cpp \
                    $(PRJ_SRC)/NodeAnimationTrack.cpp \
                    $(PRJ_SRC)/OgreExceptionManager.cpp \
                    $(PRJ_SRC)/OgreInterface.cpp \
                    $(PRJ_SRC)/OgreLogListener.cpp \
                    $(PRJ_SRC)/OgreManagedArchive.cpp \
                    $(PRJ_SRC)/OgreManagedArchiveFactory.cpp \
                    $(PRJ_SRC)/OgreManagedStream.cpp \
                    $(PRJ_SRC)/OgreResourceGroupManager.cpp \
                    $(PRJ_SRC)/Pass.cpp \
                    $(PRJ_SRC)/PixelBox.cpp \
                    $(PRJ_SRC)/Pose.cpp \
                    $(PRJ_SRC)/RenderQueue.cpp \
                    $(PRJ_SRC)/RenderSystem.cpp \
                    $(PRJ_SRC)/RenderTarget.cpp \
                    $(PRJ_SRC)/RenderWindow.cpp \
                    $(PRJ_SRC)/Resource.cpp \
					$(PRJ_SRC)/Root.cpp \
                    $(PRJ_SRC)/SceneManager.cpp \
                    $(PRJ_SRC)/SceneNode.cpp \
                    $(PRJ_SRC)/Skeleton.cpp \
                    $(PRJ_SRC)/SkeletonManager.cpp \
                    $(PRJ_SRC)/StaticGeometry.cpp \
                    $(PRJ_SRC)/SubEntity.cpp \
                    $(PRJ_SRC)/SubMesh.cpp \
                    $(PRJ_SRC)/Technique.cpp \
                    $(PRJ_SRC)/Texture.cpp \
                    $(PRJ_SRC)/TextureManager.cpp \
                    $(PRJ_SRC)/TextureUnitState.cpp \
                    $(PRJ_SRC)/VertexAnimationTrack.cpp \
                    $(PRJ_SRC)/VertexBufferBinding.cpp \
                    $(PRJ_SRC)/VertexData.cpp \
                    $(PRJ_SRC)/VertexDeclaration.cpp \
                    $(PRJ_SRC)/VertexElement.cpp \
                    $(PRJ_SRC)/VertexPoseKeyFrame.cpp \
                    $(PRJ_SRC)/Viewport.cpp \
                    $(PRJ_SRC)/WindowEventUtilities.cpp \
                    $(PRJ_SRC)/WrapperMath.cpp \

LOCAL_LDLIBS    := -llog -landroid
LOCAL_LDLIBS	+= -lc -lm -ldl
LOCAL_STATIC_LIBRARIES := rendersystem_gles2 ogremain
LOCAL_STATIC_LIBRARIES += freetype FreeImage zlib zzip libTegra_gfx_debugger
LOCAL_STATIC_LIBRARIES += android_native_app_glue cpufeatures

include $(BUILD_SHARED_LIBRARY)

$(call import-module,android/cpufeatures)
$(call import-module,android/native_app_glue)