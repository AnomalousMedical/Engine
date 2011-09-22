using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class Light : MovableObject
    {
        [SingleEnum]
        public enum LightTypes : uint
        {
	        LT_POINT,
	        LT_DIRECTIONAL,
	        LT_SPOTLIGHT
        };

        internal static Light createWrapper(IntPtr light, object[] args)
        {
            return new Light(light);
        }

        private Light(IntPtr light)
            :base(light)
        {

        }

        /// <summary>
        /// Sets the type of light - see LightTypes for more info.
        /// </summary>
        /// <param name="type">The type of light.</param>
        public void setType(LightTypes type)
        {
            Light_setType(ogreObject, type);
        }

        /// <summary>
        /// Returns the light type.
        /// </summary>
        /// <returns>The LightTypes of the light.</returns>
        public LightTypes getType()
        {
            return Light_getType(ogreObject);
        }

        /// <summary>
        /// Sets the colour of the diffuse light given off by this source.
        /// </summary>
        /// <param name="red">Red color component.</param>
        /// <param name="green">Green color component.</param>
        /// <param name="blue">Blue color component.</param>
        public void setDiffuseColor(float red, float green, float blue)
        {
            Light_setDiffuseColorRaw(ogreObject, red, green, blue);
        }

        /// <summary>
        /// Sets the colour of the diffuse light given off by this source.
        /// </summary>
        /// <param name="color">A vector3 with the color info.  x=red, y=green, z=blue</param>
        public void setDiffuseColor(Color color)
        {
            Light_setDiffuseColor(ogreObject, color);
        }

        /// <summary>
        /// Get the diffuse color of the light.
        /// </summary>
        public Color getDiffuseColor()
        {
            return Light_getDiffuseColor(ogreObject);
        }

        /// <summary>
        /// Sets the colour of the specular light given off by this source.
        /// </summary>
        /// <param name="red">Red color component.</param>
        /// <param name="green">Green color component.</param>
        /// <param name="blue">Blue color component.</param>
        public void setSpecularColor(float red, float green, float blue)
        {
            Light_setSpecularColorRaw(ogreObject, red, green, blue);
        }

        /// <summary>
        /// Sets the colour of the specular light given off by this source.
        /// </summary>
        /// <param name="color">A vector3 with the color info.  x=red, y=green, z=blue</param>
        public void setSpecularColor(Color color)
        {
            Light_setSpecularColor(ogreObject, color);
        }

        /// <summary>
        /// Get the specular color of the light.
        /// </summary>
        public Color getSpecularColor()
        {
            return Light_getSpecularColor(ogreObject);
        }

        /// <summary>
        /// Sets the attenuation parameters of the light source ie how it diminishes with distance.
        ///
        /// Remarks:
        /// Lights normally get fainter the further they are away. Also, each light is given a 
        /// maximum range beyond which it cannot affect any objects. 
        ///
        /// Light attentuation is not applicable to directional lights since they have an 
        /// infinite range and constant intensity.
        /// </summary>
        /// <param name="range">The absolute upper range of the light in world units.</param>
        /// <param name="constant">The constant factor in the attenuation formula: 1.0 means never attenuate, 0.0 is complete attenuation.</param>
        /// <param name="linear">The linear factor in the attenuation formula: 1 means attenuate evenly over the distance.</param>
        /// <param name="quadratic">The quadratic factor in the attenuation formula: adds a curvature to the attenuation formula.</param>
        public void setAttenuation(float range, float constant, float linear, float quadratic)
        {
            Light_setAttenuation(ogreObject, range, constant, linear, quadratic);
        }

        /// <summary>
        /// Returns the absolute upper range of the light.
        /// </summary>
        /// <returns>Returns the absolute upper range of the light.</returns>
        public float getAttenuationRange()
        {
            return Light_getAttenuationRange(ogreObject);
        }

        /// <summary>
        /// Returns the constant factor in the attenuation formula.
        /// </summary>
        /// <returns>Returns the constant factor in the attenuation formula.</returns>
        public float getAttenuationConstant()
        {
            return Light_getAttenuationConstant(ogreObject);
        }

        /// <summary>
        /// Returns the linear factor in the attenuation formula.
        /// </summary>
        /// <returns>Returns the linear factor in the attenuation formula.</returns>
        public float getAttenuationLinear()
        {
            return Light_getAttenuationLinear(ogreObject);
        }

        /// <summary>
        /// Returns the quadric factor in the attenuation formula.
        /// </summary>
        /// <returns>Returns the quadric factor in the attenuation formula.</returns>
        public float getAttenuationQuadric()
        {
            return Light_getAttenuationQuadric(ogreObject);
        }

        /// <summary>
        /// Sets the position of the light. Applicable to point lights and
        /// spotlights only. This will be overridden if the light is attached to a
        /// SceneNode. 
        /// </summary>
        /// <param name="pos">The position to set.</param>
        public void setPosition(Vector3 pos)
        {
            Light_setPosition(ogreObject, pos);
        }

        /// <summary>
        /// Sets the position of the light. Applicable to point lights and
        /// spotlights only. This will be overridden if the light is attached to a
        /// SceneNode. 
        /// </summary>
        /// <param name="x">The x translation.</param>
        /// <param name="y">The y translation.</param>
        /// <param name="z">The z translation.</param>
        public void setPosition(float x, float y, float z)
        {
            Light_setPositionRaw(ogreObject, x, y, z);
        }

        /// <summary>
        /// Returns the position of the light. Applicable to point lights and
        /// spotlights only.
        /// </summary>
        /// <returns></returns>
        public Vector3 getPosition()
        {
            return Light_getPosition(ogreObject);
        }

        /// <summary>
        /// Sets the direction in which a light points.
        /// </summary>
        /// <param name="x">X Direction.</param>
        /// <param name="y">Y Direction.</param>
        /// <param name="z">Z Direction.</param>
        public void setDirection(float x, float y, float z)
        {
            Light_setDirectionRaw(ogreObject, x, y, z);
        }

        /// <summary>
        /// Sets the direction in which a light points.
        /// </summary>
        /// <param name="dir">A vector containing the direction.</param>
        public void setDirection(Vector3 dir)
        {
            Light_setDirection(ogreObject, dir);
        }

        /// <summary>
        /// Get the direction the light is facing.
        /// </summary>
        public Vector3 getDirection()
        {
            return Light_getDirection(ogreObject);
        }

        /// <summary>
        /// Sets the range of a spotlight, i.e.
        /// 
        /// the angle of the inner and outer cones and the rate of falloff between them
        /// </summary>
        /// <param name="innerAngleRad">Angle covered by the bright inner cone The inner cone applicable only to Direct3D, it'll always treat as zero in OpenGL.</param>
        /// <param name="outerAngleRad">Angle covered by the outer cone.</param>
        /// <param name="falloff">The rate of falloff between the inner and outer cones. 1.0 means a linear falloff, less means slower falloff, higher means faster falloff.</param>
        public void setSpotlightRange(float innerAngleRad, float outerAngleRad, float falloff)
        {
            Light_setSpotlightRange(ogreObject, innerAngleRad, outerAngleRad, falloff);
        }

        /// <summary>
        /// Returns the angle covered by the spotlights inner cone.
        /// </summary>
        /// <returns>Returns the angle covered by the spotlights inner cone.</returns>
        public float getSpotlightInnerAngle()
        {
            return Light_getSpotlightInnerAngle(ogreObject);
        }

        /// <summary>
        /// Returns the angle covered by the spotlights outer cone.
        /// </summary>
        /// <returns>Returns the angle covered by the spotlights outer cone.</returns>
        public float getSpotlightOuterAngle()
        {
            return Light_getSpotlightOuterAngle(ogreObject);
        }

        /// <summary>
        /// Returns the falloff between the inner and outer cones of the spotlight.
        /// </summary>
        /// <returns>Returns the falloff between the inner and outer cones of the spotlight.</returns>
        public float getSpotlightFalloff()
        {
            return Light_getSpotlightFalloff(ogreObject);
        }

        /// <summary>
        /// Sets the angle covered by the spotlights inner cone.
        /// </summary>
        /// <param name="innerAngleRad">Angle covered by the bright inner cone The inner cone applicable only to Direct3D, it'll always treat as zero in OpenGL.</param>
        public void setSpotlightInnerAngle(float innerAngleRad)
        {
            Light_setSpotlightInnerAngle(ogreObject, innerAngleRad);
        }

        /// <summary>
        /// Sets the angle covered by the spotlights outer cone.
        /// </summary>
        /// <param name="outerAngleRad">Angle covered by the outer cone.</param>
        public void setSpotlightOuterAngle(float outerAngleRad)
        {
            Light_setSpotlightOuterAngle(ogreObject, outerAngleRad);
        }

        /// <summary>
        /// Sets the falloff between the inner and outer cones of the spotlight.
        /// </summary>
        /// <param name="value">The rate of falloff between the inner and outer cones. 1.0 means a linear falloff, less means slower falloff, higher means faster falloff.</param>
        public void setSpotlightFalloff(float value)
        {
            Light_setSpotlightFalloff(ogreObject, value);
        }

        /// <summary>
        /// Set a scaling factor to indicate the relative power of a light.
        /// This factor is only useful in High Dynamic Range (HDR) rendering. You can bind it 
        /// to a shader variable to take it into account.
        /// </summary>
        /// <param name="power">The power rating of this light, default is 1.0.</param>
        public void setPowerScale(float power)
        {
            Light_setPowerScale(ogreObject, power);
        }

        /// <summary>
        /// Get the scaling factor which indicates the relative power of a light.
        /// </summary>
        /// <returns>Get the scaling factor which indicates the relative power of a light.</returns>
        public float getPowerScale()
        {
            return Light_getPowerScale(ogreObject);
        }

        public void setCastShadows(bool cast)
        {
            Light_setCastShadows(ogreObject, cast);
        }

        public bool getCastShadows()
        {
            return Light_getCastShadows(ogreObject);
        }

        #region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setType(IntPtr light, LightTypes type);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern LightTypes Light_getType(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setDiffuseColorRaw(IntPtr light, float red, float green, float blue);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setDiffuseColor(IntPtr light, Color color);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Light_getDiffuseColor(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setSpecularColorRaw(IntPtr light, float red, float green, float blue);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setSpecularColor(IntPtr light, Color color);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Light_getSpecularColor(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setAttenuation(IntPtr light, float range, float constant, float linear, float quadratic);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getAttenuationRange(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getAttenuationConstant(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getAttenuationLinear(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getAttenuationQuadric(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setPosition(IntPtr light, Vector3 pos);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setPositionRaw(IntPtr light, float x, float y, float z);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Light_getPosition(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setDirectionRaw(IntPtr light, float x, float y, float z);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setDirection(IntPtr light, Vector3 dir);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Light_getDirection(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setSpotlightRange(IntPtr light, float innerAngleRad, float outerAngleRad, float falloff);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getSpotlightInnerAngle(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getSpotlightOuterAngle(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getSpotlightFalloff(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setSpotlightInnerAngle(IntPtr light, float innerAngleRad);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setSpotlightOuterAngle(IntPtr light, float outerAngleRad);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setSpotlightFalloff(IntPtr light, float value);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setPowerScale(IntPtr light, float power);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Light_getPowerScale(IntPtr light);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Light_setCastShadows(IntPtr light, bool cast);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Light_getCastShadows(IntPtr light);

        #endregion 
    }
}
