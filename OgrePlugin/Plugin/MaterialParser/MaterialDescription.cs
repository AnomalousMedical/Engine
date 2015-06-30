using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class MaterialDescription
    {
        Color specularColor;
        Color emissiveColor;
        Color diffuseColor;

        public MaterialDescription()
        {
            emissiveColor = Color.Black;
            specularColor = Color.White;
            Shinyness = 40.0f;
            diffuseColor = Color.White;
            
            CreateAlphaMaterial = true;
            NumHardwareBones = 0;
            NumHardwarePoses = 0;
            GlossyStart = 40;
            GlossyRange = 0;
            Parity = false;
        }

        public String Name { get; set; }

        public String Builder { get; set; }

        public String ShaderName { get; set; }

        public String Emissive
        {
            get
            {
                return emissiveColor.ToHexRGBAString();
            }
            set
            {
                if(!Color.TryFromRGBAString(value, out emissiveColor, Color.HotPink))
                {
                    Logging.Log.Error("Could not parse emissive color '{0}' for material '{1}'", value, Name);
                }
            }
        }

        public String Specular
        {
            get
            {
                return specularColor.ToHexRGBAString();
            }
            set
            {
                if (!Color.TryFromRGBAString(value, out specularColor, Color.HotPink))
                {
                    Logging.Log.Error("Could not parse specular color '{0}' for material '{1}'", value, Name);
                }
            }
        }

        public float Shinyness { get; set; }

        public String Diffuse
        {
            get
            {
                return diffuseColor.ToHexRGBAString();
            }
            set
            {
                if (!Color.TryFromRGBAString(value, out diffuseColor, Color.HotPink))
                {
                    Logging.Log.Error("Could not parse diffuse color '{0}' for material '{1}'", value, Name);
                }
            }
        }

        public float GlossyStart { get; set; }

        public float GlossyRange { get; set; }

        public String NormalMap { get; set; }

        public String DiffuseMap { get; set; }

        public String SpecularMap { get; set; }

        public String OpacityMap { get; set; }

        public bool CreateAlphaMaterial { get; set; }

        public bool Parity { get; set; }

        public int NumHardwareBones { get; set; }

        public int NumHardwarePoses { get; set; }

        //Filled in at runtime
        public String Group { get; set; }

        public String SourceFile { get; set; }

        public Color EmissiveColor
        {
            get
            {
                return emissiveColor;
            }
            set
            {
                emissiveColor = value;
            }
        }

        public Color DiffuseColor
        {
            get
            {
                return diffuseColor;
            }
            set
            {
                diffuseColor = value;
            }
        }

        public Color SpecularColor
        {
            get
            {
                return specularColor;
            }
            set
            {
                specularColor = value;
            }
        }

        public String TextureSet
        {
            get
            {
                return String.Format("{0}|{1}|{2}|{3}-{4}", NormalMap, DiffuseMap, SpecularMap, OpacityMap, ShaderName);
            }
        }

        public bool HasTextures
        {
            get
            {
                return NormalMap != null || DiffuseMap != null || SpecularMap != null || OpacityMap != null;
            }
        }
    }
}
