using Engine;
using Engine.Editing;
using Engine.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
    public partial class MaterialDescription
    {
        Color specularColor;
        Color emissiveColor;
        Color diffuseColor;
        float? opacity;
        MaterialDescription parent;

        public MaterialDescription()
        {
            emissiveColor = Color.Black;
            specularColor = Color.White;
            Shinyness = 40.0f;
            diffuseColor = Color.White;
            opacity = null;
            
            CreateAlphaMaterial = true;
            IsAlpha = false;
            NumHardwareBones = 0;
            NumHardwarePoses = 0;
            HasNormalMap = false;
            HasDiffuseMap = false;
            HasSpecularColorMap = false;
            HasSpecularLevelMap = false;
            HasGlossMap = false;
            HasOpacityMap = false;
            GlossyStart = 40;
            GlossyRange = 0;
            Parity = false;
            Variants = new List<MaterialDescription>();
            KeepHighestMipLoaded = false;
            SpecialMaterial = null;
            IsHighlight = false;
            NoDepthWriteAlpha = false;
            DisableBackfaceCulling = false;
        }

        private MaterialDescription(MaterialDescription toClone)
        {
            this.parent = toClone;
            this.Name = toClone.Name;
            this.Builder = toClone.Builder;
            this.TextureSet = toClone.TextureSet;
            this.Shinyness = toClone.Shinyness;
            this.HasGlossMap = toClone.HasGlossMap;
            this.GlossyStart = toClone.GlossyStart;
            this.GlossyRange = toClone.GlossyRange;
            this.HasNormalMap = toClone.HasNormalMap;
            this.HasDiffuseMap = toClone.HasDiffuseMap;
            this.HasSpecularColorMap = toClone.HasSpecularColorMap;
            this.HasSpecularLevelMap = toClone.HasSpecularLevelMap;
            this.HasGlossMap = toClone.HasGlossMap;
            this.HasOpacityMap = toClone.HasOpacityMap;
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
            this.opacity = toClone.opacity;
            this.KeepHighestMipLoaded = toClone.KeepHighestMipLoaded;
            this.SpecialMaterial = toClone.SpecialMaterial;
            this.IsHighlight = toClone.IsHighlight;
            this.NoDepthWriteAlpha = toClone.NoDepthWriteAlpha;
            this.DisableBackfaceCulling = toClone.DisableBackfaceCulling;
        }

        public String localizePath(String path)
        {
            return Path.Combine(Path.GetDirectoryName(SourceFile), path);
        }

        [Editable]
        [JsonProperty]
        public String Name { get; set; }

        [Editable]
        [JsonProperty]
        public String Builder { get; set; }

        [Editable]
        [JsonProperty]
        public String TextureSet { get; set; }

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

        [Editable]
        [JsonProperty]
        public String Opacity
        {
            get
            {
                return opacity != null ? opacity.ToString() : null;
            }
            set
            {
                float parsed;
                if(value != null && NumberParser.TryParse(value, out parsed))
                {
                    opacity = parsed;
                }
                else
                {
                    opacity = null;
                }
            }
        }

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

        [Editable]
        [JsonProperty]
        public float Shinyness { get; set; }

        [Editable]
        [JsonProperty]
        public float GlossyStart { get; set; }

        [Editable]
        [JsonProperty]
        public float GlossyRange { get; set; }

        [Editable]
        [JsonProperty]
        public bool HasNormalMap { get; set; }

        [Editable]
        [JsonProperty]
        public bool HasDiffuseMap { get; set; }

        [Editable]
        [JsonProperty]
        public bool HasSpecularColorMap { get; set; }

        [Editable]
        [JsonProperty]
        public bool HasSpecularLevelMap { get; set; }

        [Editable]
        [JsonProperty]
        public bool HasGlossMap { get; set; }

        [Editable]
        [JsonProperty]
        public bool HasOpacityMap { get; set; }

        [Editable]
        [JsonProperty]
        public bool CreateAlphaMaterial { get; set; }

        [Editable]
        [JsonProperty]
        public bool IsAlpha { get; set; }

        [Editable]
        [JsonProperty]
        public bool Parity { get; set; }

        [EditableMinMax(0, 4, 1)]
        [JsonProperty]
        public int NumHardwareBones { get; set; }

        [EditableMinMax(0, 2, 1)]
        [JsonProperty]
        public int NumHardwarePoses { get; set; }

        [Editable]
        [JsonProperty]
        public bool IsHighlight { get; set; }

        [Editable]
        [JsonProperty]
        public bool NoDepthWriteAlpha { get; set; }

        [Editable]
        [JsonProperty]
        public bool DisableBackfaceCulling { get; set; }

        [JsonProperty]
        private List<MaterialDescription> Variants { get; set; } //Cheating with this, forcing it to work as a property to not fight json.net for now

        [Editable]
        [JsonProperty]
        public bool KeepHighestMipLoaded { get; set; }

        [Editable]
        [JsonProperty]
        public String SpecialMaterial { get; set; }

        //Filled in at runtime

        /// <summary>
        /// Get all nested descriptions for all levels below this one.
        /// </summary>
        internal IEnumerable<MaterialDescription> AllVariants
        {
            get
            {
                if (Variants == null)
                {
                    return IEnumerableUtil<MaterialDescription>.EmptyIterator;
                }

                IEnumerable<MaterialDescription> retVal = Variants;
                foreach (var child in Variants)
                {
                    if (child.Variants != null) //Avoids concating empty iterators
                    {
                        retVal.Concat(child.AllVariants);
                    }
                }
                return retVal;
            }
        }

        public String Group { get; set; }

        public String SourceFile { get; set; }

        public bool IsSpecialMaterial
        {
            get
            {
                return !String.IsNullOrEmpty(SpecialMaterial);
            }
        }

        [Editable]
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

        [Editable]
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

        [Editable]
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

        public bool HasOpacityValue
        {
            get
            {
                return opacity != null;
            }
        }

        public float OpacityValue
        {
            get
            {
                return opacity.Value;
            }
        }

        public String NormalMapName
        {
            get
            {
                return String.Format("{0}Normal", TextureSet);
            }
        }

        public String DiffuseMapName
        {
            get
            {
                return String.Format("{0}Diffuse", TextureSet);
            }
        }

        public String SpecularMapName
        {
            get
            {
                return String.Format("{0}Specular", TextureSet);
            }
        }

        public String OpacityMapName
        {
            get
            {
                return String.Format("{0}Opacity", TextureSet);
            }
        }

        public String SpecularLevelMapName
        {
            get
            {
                return String.Format("{0}SpecularLevel", TextureSet);
            }
        }

        public String GlossMapName
        {
            get
            {
                return String.Format("{0}Gloss", TextureSet);
            }
        }

        public bool IsRoot
        {
            get
            {
                return parent == null;
            }
        }

        /// <summary>
        /// This is a filter that finds only fields marked with the
        /// EditableAttribute attribute.
        /// </summary>
        public class JsonPropertyFilter : MemberScannerFilter
        {
            /// <summary>
            /// Initializes a new Instance of Engine.Editing.EditableAttributeFilter
            /// </summary>
            public JsonPropertyFilter()
            {

            }

            /// <summary>
            /// This is the test function. It will return true if the member should
            /// be accepted.
            /// </summary>
            /// <param name="wrapper">The MemberWrapper with info about the field/property being scanned.</param>
            /// <returns>True if the member should be included in the results. False to omit it.</returns>
            public bool allowMember(MemberWrapper wrapper)
            {
                return wrapper.getCustomAttributes(typeof(JsonPropertyAttribute), true).Any();
            }

            /// <summary>
            /// This function determines if the given type should be scanned for
            /// members. It will return true if the member should be accepted.
            /// </summary>
            /// <param name="type">The type to potentially scan for members.</param>
            /// <returns>True if the type should be scanned.</returns>
            public bool allowType(Type type)
            {
                return type != TerminatingType;
            }

            /// <summary>
            /// This is the type the filter will stop allowing types for.
            /// </summary>
            public Type TerminatingType { get; set; }
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
                using (var jObjectReader = jObject.CreateReader())
                {
                    serializer.Populate(jObjectReader, description);
                }
                parent = myParent;

                return description;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                //No writing support for now
                MaterialDescription desc = value as MaterialDescription;
                bool alwaysWrite = desc.IsRoot;
                FilteredMemberScanner scanner = new FilteredMemberScanner(new JsonPropertyFilter());
                scanner.ProcessFields = false;
                writer.WriteStartObject();
                foreach(var property in scanner.getMatchingMembers(typeof(MaterialDescription)))
                {
                    Object thisValue = property.getValue(desc, null);
                    Object parentValue = property.getValue(desc.parent, null);
                    if (alwaysWrite || (thisValue != null && !thisValue.Equals(parentValue)))
                    {
                        if (property.getWrappedName() != "Variants")
                        {
                            writer.WritePropertyName(property.getWrappedName());
                            writer.WriteValue(thisValue);
                        }
                    }
                }
                if (desc.Variants != null && desc.Variants.Count > 0)
                {
                    writer.WritePropertyName("Variants");
                    writer.WriteStartArray();
                    foreach (var variant in desc.Variants)
                    {
                        serializer.Serialize(writer, variant);
                    }
                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }
        }
    }

    public partial class MaterialDescription
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if(editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, Name);
                if(Variants == null) //Always have this collection in edit mode
                {
                    Variants = new List<MaterialDescription>();
                }

                var materialDescManager = editInterface.createEditInterfaceManager<MaterialDescription>();
                materialDescManager.addCommand(new EditInterfaceCommand("Remove", callback =>
                {
                    MaterialDescription desc = editInterface.resolveSourceObject<MaterialDescription>(callback.getSelectedEditInterface());
                    Variants.Remove(desc);
                    editInterface.removeSubInterface(desc);
                }));

                editInterface.addCommand(new EditInterfaceCommand("Add Variant", callback =>
                {
                    MaterialDescription newDesc = new MaterialDescription();
                    Variants.Add(newDesc);
                    editInterface.addSubInterface(newDesc, newDesc.getEditInterface());
                }));

                foreach(var variant in Variants)
                {
                    editInterface.addSubInterface(variant, variant.getEditInterface());
                }
            }
            return editInterface;
        }
    }
}
