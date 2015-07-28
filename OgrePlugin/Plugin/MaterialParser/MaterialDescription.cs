﻿using Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(MaterialDescription.MaterialDescriptionSerializer))]
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
            IsAlpha = false;
            NumHardwareBones = 0;
            NumHardwarePoses = 0;
            GlossyStart = 40;
            GlossyRange = 0;
            Parity = false;
            Variants = new List<MaterialDescription>();
        }

        private MaterialDescription(MaterialDescription toClone)
        {
            this.Name = toClone.Name;
            this.Builder = toClone.Builder;
            this.ShaderName = toClone.ShaderName;
            this.Shinyness = toClone.Shinyness;
            this.GlossyStart = toClone.GlossyStart;
            this.GlossyRange = toClone.GlossyRange;
            this.NormalMap = toClone.NormalMap;
            this.DiffuseMap = toClone.DiffuseMap;
            this.SpecularMap = toClone.SpecularMap;
            this.OpacityMap = toClone.OpacityMap;
            this.CreateAlphaMaterial = toClone.CreateAlphaMaterial;
            this.IsAlpha = toClone.IsAlpha;
            this.Parity = toClone.Parity;
            this.NumHardwareBones = toClone.NumHardwareBones;
            this.NumHardwarePoses = toClone.NumHardwarePoses;
            this.Group = toClone.Group;
            this.SourceFile = toClone.SourceFile;
            this.specularColor = toClone.specularColor;
            this.emissiveColor = toClone.emissiveColor;
            this.diffuseColor = toClone.diffuseColor;
        }

        public String localizePath(String path)
        {
            return Path.Combine(Path.GetDirectoryName(SourceFile), path);
        }

        [JsonProperty]
        public String Name { get; set; }

        [JsonProperty]
        public String Builder { get; set; }

        [JsonProperty]
        public String ShaderName { get; set; }

        [JsonProperty]
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

        [JsonProperty]
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

        [JsonProperty]
        public float Shinyness { get; set; }

        [JsonProperty]
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

        [JsonProperty]
        public float GlossyStart { get; set; }

        [JsonProperty]
        public float GlossyRange { get; set; }

        [JsonProperty]
        public String NormalMap { get; set; }

        [JsonProperty]
        public String DiffuseMap { get; set; }

        [JsonProperty]
        public String SpecularMap { get; set; }

        [JsonProperty]
        public String OpacityMap { get; set; }

        [JsonProperty]
        public bool CreateAlphaMaterial { get; set; }

        [JsonProperty]
        public bool IsAlpha { get; set; }

        [JsonProperty]
        public bool Parity { get; set; }

        [JsonProperty]
        public int NumHardwareBones { get; set; }

        [JsonProperty]
        public int NumHardwarePoses { get; set; }

        [JsonProperty]
        private List<MaterialDescription> Variants { get; set; } //Cheating with this, forcing it to work as a property to not fight json.net for now

        /// <summary>
        /// Get all nested descriptions for all levels below this one.
        /// </summary>
        internal IEnumerable<MaterialDescription> AllVariants
        {
            get
            {
                if(Variants == null)
                {
                    return IEnumerableUtil<MaterialDescription>.EmptyIterator;
                }

                IEnumerable<MaterialDescription> retVal = Variants;
                foreach(var child in Variants)
                {
                    if (child.Variants != null) //Avoids concating empty iterators
                    {
                        retVal.Concat(child.AllVariants);
                    }
                }
                return retVal;
            }
        }

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
                return String.Format("{0}-{1}-{2}-{3}-{4}", NormalMap, DiffuseMap, SpecularMap, OpacityMap, ShaderName);
            }
        }

        class MaterialDescriptionSerializer : JsonConverter
        {
            MaterialDescription parent = null;

            public override bool CanConvert(Type objectType)
            {
                return typeof(MaterialDescription).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                MaterialDescription description;
                if (parent == null)
                {
                    description = new MaterialDescription();
                }
                else
                {
                    description = new MaterialDescription(parent);
                }

                MaterialDescription myParent = parent;
                parent = description;
                var jObject = JObject.Load(reader);
                serializer.Populate(jObject.CreateReader(), description);
                parent = myParent;

                return description;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                //No writing support for now
            }
        }
    }
}