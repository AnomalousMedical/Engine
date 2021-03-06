﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public enum AutoConstantType
    {
        ACT_WORLD_MATRIX,
        ACT_INVERSE_WORLD_MATRIX,
        ACT_TRANSPOSE_WORLD_MATRIX,
        ACT_INVERSE_TRANSPOSE_WORLD_MATRIX,

        ACT_WORLD_MATRIX_ARRAY_3x4,
        ACT_WORLD_MATRIX_ARRAY,
        ACT_WORLD_DUALQUATERNION_ARRAY_2x4,
        ACT_WORLD_SCALE_SHEAR_MATRIX_ARRAY_3x4,

        ACT_VIEW_MATRIX,
        ACT_INVERSE_VIEW_MATRIX,
        ACT_TRANSPOSE_VIEW_MATRIX,
        ACT_INVERSE_TRANSPOSE_VIEW_MATRIX,


        ACT_PROJECTION_MATRIX,
        ACT_INVERSE_PROJECTION_MATRIX,
        ACT_TRANSPOSE_PROJECTION_MATRIX,
        ACT_INVERSE_TRANSPOSE_PROJECTION_MATRIX,


        ACT_VIEWPROJ_MATRIX,
        ACT_INVERSE_VIEWPROJ_MATRIX,
        ACT_TRANSPOSE_VIEWPROJ_MATRIX,
        ACT_INVERSE_TRANSPOSE_VIEWPROJ_MATRIX,


        ACT_WORLDVIEW_MATRIX,
        ACT_INVERSE_WORLDVIEW_MATRIX,
        ACT_TRANSPOSE_WORLDVIEW_MATRIX,
        ACT_INVERSE_TRANSPOSE_WORLDVIEW_MATRIX,


        ACT_WORLDVIEWPROJ_MATRIX,
        ACT_INVERSE_WORLDVIEWPROJ_MATRIX,
        ACT_TRANSPOSE_WORLDVIEWPROJ_MATRIX,
        ACT_INVERSE_TRANSPOSE_WORLDVIEWPROJ_MATRIX,



        ACT_RENDER_TARGET_FLIPPING,

        ACT_VERTEX_WINDING,

        ACT_FOG_COLOUR,
        ACT_FOG_PARAMS,


        ACT_SURFACE_AMBIENT_COLOUR,
        ACT_SURFACE_DIFFUSE_COLOUR,
        ACT_SURFACE_SPECULAR_COLOUR,
        ACT_SURFACE_EMISSIVE_COLOUR,
        ACT_SURFACE_SHININESS,
        ACT_SURFACE_ALPHA_REJECTION_VALUE,


        ACT_LIGHT_COUNT,


        ACT_AMBIENT_LIGHT_COLOUR,

        ACT_LIGHT_DIFFUSE_COLOUR,
        ACT_LIGHT_SPECULAR_COLOUR,
        ACT_LIGHT_ATTENUATION,
        ACT_SPOTLIGHT_PARAMS,
        ACT_LIGHT_POSITION,
        ACT_LIGHT_POSITION_OBJECT_SPACE,
        ACT_LIGHT_POSITION_VIEW_SPACE,
        ACT_LIGHT_DIRECTION,
        ACT_LIGHT_DIRECTION_OBJECT_SPACE,
        ACT_LIGHT_DIRECTION_VIEW_SPACE,
        ACT_LIGHT_DISTANCE_OBJECT_SPACE,
        ACT_LIGHT_POWER_SCALE,
        ACT_LIGHT_DIFFUSE_COLOUR_POWER_SCALED,
        ACT_LIGHT_SPECULAR_COLOUR_POWER_SCALED,
        ACT_LIGHT_DIFFUSE_COLOUR_ARRAY,
        ACT_LIGHT_SPECULAR_COLOUR_ARRAY,
        ACT_LIGHT_DIFFUSE_COLOUR_POWER_SCALED_ARRAY,
        ACT_LIGHT_SPECULAR_COLOUR_POWER_SCALED_ARRAY,
        ACT_LIGHT_ATTENUATION_ARRAY,
        ACT_LIGHT_POSITION_ARRAY,
        ACT_LIGHT_POSITION_OBJECT_SPACE_ARRAY,
        ACT_LIGHT_POSITION_VIEW_SPACE_ARRAY,
        ACT_LIGHT_DIRECTION_ARRAY,
        ACT_LIGHT_DIRECTION_OBJECT_SPACE_ARRAY,
        ACT_LIGHT_DIRECTION_VIEW_SPACE_ARRAY,
        ACT_LIGHT_DISTANCE_OBJECT_SPACE_ARRAY,
        ACT_LIGHT_POWER_SCALE_ARRAY,
        ACT_SPOTLIGHT_PARAMS_ARRAY,

        ACT_DERIVED_AMBIENT_LIGHT_COLOUR,
        ACT_DERIVED_SCENE_COLOUR,

        ACT_DERIVED_LIGHT_DIFFUSE_COLOUR,
        ACT_DERIVED_LIGHT_SPECULAR_COLOUR,

        ACT_DERIVED_LIGHT_DIFFUSE_COLOUR_ARRAY,
        ACT_DERIVED_LIGHT_SPECULAR_COLOUR_ARRAY,
        ACT_LIGHT_NUMBER,
        ACT_LIGHT_CASTS_SHADOWS,
        ACT_LIGHT_CASTS_SHADOWS_ARRAY,


        ACT_SHADOW_EXTRUSION_DISTANCE,
        ACT_CAMERA_POSITION,
        ACT_CAMERA_POSITION_OBJECT_SPACE,
        ACT_TEXTURE_VIEWPROJ_MATRIX,
        ACT_TEXTURE_VIEWPROJ_MATRIX_ARRAY,
        ACT_TEXTURE_WORLDVIEWPROJ_MATRIX,
        ACT_TEXTURE_WORLDVIEWPROJ_MATRIX_ARRAY,
        ACT_SPOTLIGHT_VIEWPROJ_MATRIX,
        ACT_SPOTLIGHT_VIEWPROJ_MATRIX_ARRAY,
        ACT_SPOTLIGHT_WORLDVIEWPROJ_MATRIX,
        ACT_SPOTLIGHT_WORLDVIEWPROJ_MATRIX_ARRAY,
        ACT_CUSTOM,
        ACT_TIME,
        ACT_TIME_0_X,
        ACT_COSTIME_0_X,
        ACT_SINTIME_0_X,
        ACT_TANTIME_0_X,
        ACT_TIME_0_X_PACKED,
        ACT_TIME_0_1,
        ACT_COSTIME_0_1,
        ACT_SINTIME_0_1,
        ACT_TANTIME_0_1,
        ACT_TIME_0_1_PACKED,
        ACT_TIME_0_2PI,
        ACT_COSTIME_0_2PI,
        ACT_SINTIME_0_2PI,
        ACT_TANTIME_0_2PI,
        ACT_TIME_0_2PI_PACKED,
        ACT_FRAME_TIME,
        ACT_FPS,

        ACT_VIEWPORT_WIDTH,
        ACT_VIEWPORT_HEIGHT,
        ACT_INVERSE_VIEWPORT_WIDTH,
        ACT_INVERSE_VIEWPORT_HEIGHT,
        ACT_VIEWPORT_SIZE,


        ACT_VIEW_DIRECTION,
        ACT_VIEW_SIDE_VECTOR,
        ACT_VIEW_UP_VECTOR,
        ACT_FOV,
        ACT_NEAR_CLIP_DISTANCE,
        ACT_FAR_CLIP_DISTANCE,

        ACT_PASS_NUMBER,

        ACT_PASS_ITERATION_NUMBER,


        ACT_ANIMATION_PARAMETRIC,

        ACT_TEXEL_OFFSETS,

        ACT_SCENE_DEPTH_RANGE,

        ACT_SHADOW_SCENE_DEPTH_RANGE,

        ACT_SHADOW_SCENE_DEPTH_RANGE_ARRAY,

        ACT_SHADOW_COLOUR,
        ACT_TEXTURE_SIZE,
        ACT_INVERSE_TEXTURE_SIZE,
        ACT_PACKED_TEXTURE_SIZE,

        ACT_TEXTURE_MATRIX,

        ACT_LOD_CAMERA_POSITION,
        ACT_LOD_CAMERA_POSITION_OBJECT_SPACE,
        ACT_LIGHT_CUSTOM,

        ACT_UNKNOWN = 999
    };
}
