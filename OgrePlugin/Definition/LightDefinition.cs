using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.ObjectManagement;
using OgreWrapper;
using Engine.Editing;
using EngineMath;
using Engine.Reflection;

namespace OgrePlugin
{
    public class LightDefinition : MovableObjectDefinition
    {
        #region Static

        private static MemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static LightDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            EditableAttributeFilter filter = new EditableAttributeFilter();
            filter.TerminatingType = typeof(MovableObjectDefinition);
            memberScanner.Filter = filter;
        }

        #endregion Static

        public LightDefinition(String name)
            :base(name, "Light", null)
        {
            AttenuationConstant = 1;
            AttenuationLinear = 0;
            AttenuationQuadric = 0;
            DiffuseColor = new Color(1.0f, 1.0f, 1.0f);
            Direction = Vector3.Up;
            PowerScale = 1.0f;
            AttenuationRange = 100000.0f;
            SpecularColor = new Color(1.0f, 1.0f, 1.0f);
            SpotlightFalloff = 1.0f;
            SpotlightInnerAngle = 0.523559885f;
            SpotlightOuterAngle = 0.69813180f;
            LightType = Light.LightTypes.LT_POINT;
        }

        public LightDefinition(String name, Light light)
            :base(name, light, "Light", null)
        {
            AttenuationConstant = light.getAttenuationConstant();
            AttenuationLinear = light.getAttenuationLinear();
            AttenuationQuadric = light.getAttenuationQuadric();
            DiffuseColor = light.getDiffuseColor();
            Direction = light.getDirection();
            PowerScale = light.getPowerScale();
            AttenuationRange = light.getAttenuationRange();
            SpecularColor = light.getSpecularColor();
            SpotlightFalloff = light.getSpotlightFalloff();
            SpotlightInnerAngle = light.getSpotlightInnerAngle();
            SpotlightOuterAngle = light.getSpotlightOuterAngle();
            LightType = light.getType();
        }

        protected override void setupEditInterface(EditInterface editInterface)
        {
            ReflectedEditInterface.expandEditInterface(this, memberScanner, editInterface);
        }

        protected override MovableObject createActualProduct(SceneNodeElement element, OgreSceneManager scene, SimObjectBase simObject)
        {
            Identifier id = new Identifier(simObject.Name, Name);
            Light light = scene.createLight(id);
            light.setType(LightType);
            light.setAttenuation(AttenuationRange, AttenuationConstant, AttenuationLinear, AttenuationQuadric);
            light.setDiffuseColor(DiffuseColor);
            light.setDirection(Direction);
            light.setPowerScale(PowerScale);
            light.setSpecularColor(SpecularColor);
            if (light.getType() == Light.LightTypes.LT_SPOTLIGHT)
            {
                light.setSpotlightRange(SpotlightInnerAngle, SpotlightOuterAngle, SpotlightFalloff);
            }
            element.attachObject(id, light);
            return light;
        }

        [Editable]
        public Light.LightTypes LightType { get; set; }

        [Editable]
        public float AttenuationRange { get; set; }

        [Editable]
        public float AttenuationConstant { get; set; }

        [Editable]
        public float AttenuationLinear { get; set; }

        [Editable]
        public float AttenuationQuadric { get; set; }

        [Editable]
        public Color DiffuseColor { get; set; }

        [Editable]
        public Color SpecularColor { get; set; }

        [Editable]
        public Vector3 Direction { get; set; }

        [Editable]
        public float PowerScale { get; set; }

        [Editable]
        public float SpotlightFalloff { get; set; }

        [Editable]
        public float SpotlightInnerAngle { get; set; }

        [Editable]
        public float SpotlightOuterAngle { get; set; }

        #region Saveable

        private const String LIGHT_TYPE = "LightType";
        private const String ATTENUATION_RANGE = "AttenuationRange";
        private const String ATTENUATION_CONSTANT = "AttenuationConstant";
        private const String ATTENUATION_LINEAR = "AttenuationLinear";
        private const String ATTENUATION_QUADRIC = "AttenuationQuadric";
        private const String DIFFUSE_COLOR = "DiffuseColor";
        private const String SPECULAR_COLOR = "SpecularColor";
        private const String DIRECTION = "Direction";
        private const String POWER_SCALE = "PowerScale";
        private const String SPOTLIGHT_FALLOFF = "SpotlightFalloff";
        private const String SPOTLIGHT_INNER_ANGLE = "SpotlightInnerAngle";
        private const String SPOTLIGHT_OUTER_ANGLE = "SpotlightOuterAngle";

        private LightDefinition(LoadInfo info)
            :base(info, "Light", null)
        {
            LightType = info.GetValue <Light.LightTypes>(LIGHT_TYPE);
            AttenuationRange = info.GetFloat(ATTENUATION_RANGE);
            AttenuationConstant = info.GetFloat(ATTENUATION_CONSTANT);
            AttenuationLinear = info.GetFloat(ATTENUATION_LINEAR);
            AttenuationQuadric = info.GetFloat(ATTENUATION_QUADRIC);
            DiffuseColor = info.GetColor(DIFFUSE_COLOR);
            SpecularColor = info.GetColor(SPECULAR_COLOR);
            Direction = info.GetVector3(DIRECTION);
            PowerScale = info.GetFloat(POWER_SCALE);
            SpotlightFalloff = info.GetFloat(SPOTLIGHT_FALLOFF);
            SpotlightInnerAngle = info.GetFloat(SPOTLIGHT_INNER_ANGLE);
            SpotlightOuterAngle = info.GetFloat(SPOTLIGHT_OUTER_ANGLE);
        }

        protected override void getSpecificInfo(SaveInfo info)
        {
            info.AddValue(LIGHT_TYPE, LightType);
            info.AddValue(ATTENUATION_RANGE, AttenuationRange);
            info.AddValue(ATTENUATION_CONSTANT, AttenuationConstant);
            info.AddValue(ATTENUATION_LINEAR, AttenuationLinear);
            info.AddValue(ATTENUATION_QUADRIC, AttenuationQuadric);
            info.AddValue(DIFFUSE_COLOR, DiffuseColor);
            info.AddValue(SPECULAR_COLOR, SpecularColor);
            info.AddValue(DIRECTION, Direction);
            info.AddValue(POWER_SCALE, PowerScale);
            info.AddValue(SPOTLIGHT_FALLOFF, SpotlightFalloff);
            info.AddValue(SPOTLIGHT_INNER_ANGLE, SpotlightInnerAngle);
            info.AddValue(SPOTLIGHT_OUTER_ANGLE, SpotlightOuterAngle);
        }

        #endregion Saveable
    }
}
