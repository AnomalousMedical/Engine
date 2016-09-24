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
        private static FilteredMemberScanner scanner;

        static void ensureScannerExists()
        {
            if (scanner == null)
            {
                scanner = new FilteredMemberScanner(new JsonPropertyFilter());
                scanner.ProcessFields = false;
            }
        }

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
            EnableDepthCheck = true;
            EnableDepthWrite = true;
            SceneBlending = SceneBlendType.SBT_REPLACE;
            DepthFunction = CompareFunction.CMPF_LESS_EQUAL;
            NoDepthWriteAlpha = false;
            DisableBackfaceCulling = false;
            TilesetReferenceFile = null;
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
            this.EnableDepthCheck = toClone.EnableDepthCheck;
            this.EnableDepthWrite = toClone.EnableDepthWrite;
            this.NoDepthWriteAlpha = toClone.NoDepthWriteAlpha;
            this.SceneBlending = toClone.SceneBlending;
            this.DepthFunction = toClone.DepthFunction;
            this.DisableBackfaceCulling = toClone.DisableBackfaceCulling;
            this.TilesetReferenceFile = toClone.TilesetReferenceFile;
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

        [JsonProperty]
        public String Emissive
        {
            get
            {
                return emissiveColor.ToHexRGBAString();
            }
            set
            {
                if (!Color.TryFromRGBAString(value, out emissiveColor, Color.HotPink))
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
                if (value != null && NumberParser.TryParse(value, out parsed))
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
        public bool EnableDepthCheck { get; set; }

        [Editable]
        [JsonProperty]
        public bool EnableDepthWrite { get; set; }

        [Editable]
        [JsonProperty]
        public SceneBlendType SceneBlending { get; set; }

        [Editable]
        [JsonProperty]
        public CompareFunction DepthFunction { get; set; }

        [Editable]
        [JsonProperty]
        public bool NoDepthWriteAlpha { get; set; }

        [Editable]
        [JsonProperty]
        public bool DisableBackfaceCulling { get; set; }

        /// <summary>
        /// This is a refrence to a tileset json file that describes tile boundries to
        /// use when resizing the image. 
        /// </summary>
        [Editable]
        [JsonProperty]
        public String TilesetReferenceFile { get; set; }

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
                MaterialDescription defaultDescription = new MaterialDescription();
                MaterialDescription desc = value as MaterialDescription;
                bool isRoot = desc.IsRoot;

                writer.WriteStartObject();

                ensureScannerExists();
                foreach (var property in scanner.getMatchingMembers(typeof(MaterialDescription)))
                {
                    if (property.getWrappedName() != "Variants")
                    {
                        Object thisValue = property.getValue(desc, null);
                        bool write = false;
                        if(isRoot)
                        {
                            //If we are root only write if the value does not match the default
                            Object defaultValue = property.getValue(defaultDescription, null);
                            write = thisValue != null && !thisValue.Equals(defaultValue);
                        }
                        else
                        {
                            //If we are not root write the value if it does not match the parent value
                            Object parentValue = property.getValue(desc.parent, null);
                            write = thisValue != null && !thisValue.Equals(parentValue);
                        }

                        if (write)
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

    public partial class MaterialDescription
    {
        private EditInterface editInterface;
        private List<VariantProperties> variantProperties;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, Name, () => editInterface.setName(Name));
                variantProperties = new List<VariantProperties>();
                if (Variants == null)
                {
                    Variants = new List<MaterialDescription>();
                }

                var variantPropertiesManager = editInterface.createEditInterfaceManager<VariantProperties>();
                variantPropertiesManager.addCommand(new EditInterfaceCommand("Remove", callback =>
                {
                    VariantProperties variant = editInterface.resolveSourceObject<VariantProperties>(callback.getSelectedEditInterface());
                    variantProperties.Remove(variant);
                    editInterface.removeSubInterface(variant);
                }));

                editInterface.addCommand(new EditInterfaceCommand("Add Variant", callback =>
                {
                    VariantProperties variant = new VariantProperties();
                    variantProperties.Add(variant);
                    editInterface.addSubInterface(variant, variant.EditInterface);
                }));

                foreach (var variant in Variants)
                {
                    var properties = new VariantProperties(variant, this);
                    variantProperties.Add(properties);
                    editInterface.addSubInterface(properties, properties.EditInterface);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Rebuild the variants from their property definions, reccomended to do before saving if the edit
        /// interface has been used. Won't do anything if the edit interface has not been created.
        /// </summary>
        public void rebuildVariants()
        {
            //We use variantproperties to write variants not the variants themselves, this keeps 
            if (variantProperties != null && variantProperties.Count > 0)
            {
                Variants.Clear();
                foreach (var variantProps in variantProperties)
                {
                    MaterialDescription variant = new MaterialDescription(this);
                    variantProps.applyTo(variant);
                    Variants.Add(variant);
                }
            }
        }

        class VariantProperties
        {
            private List<VariantOverride> overrides = new List<VariantOverride>();
            private EditInterface editInterface;

            public VariantProperties()
            {
                overrides.Add(new VariantOverride("Name", "", this));
                buildEditInterface("Variant");
            }

            public VariantProperties(MaterialDescription variant, MaterialDescription parent)
            {
                ensureScannerExists();
                String name = "Variant";
                foreach (var property in scanner.getMatchingMembers(typeof(MaterialDescription)))
                {
                    Object thisValue = property.getValue(variant, null);
                    Object parentValue = property.getValue(parent, null);
                    if (thisValue != null && !thisValue.Equals(parentValue))
                    {
                        overrides.Add(new VariantOverride(property.getWrappedName(), thisValue.ToString(), this));
                        if (property.getWrappedName() == "Name")
                        {
                            name = thisValue.ToString();
                        }
                    }
                }

                buildEditInterface(name);
            }

            public void changed(VariantOverride variant)
            {
                ensureScannerExists();
                if (variant.Name == "Name")
                {
                    editInterface.setName(variant.Value);
                }
            }

            public void applyTo(MaterialDescription variant)
            {
                foreach(var o in overrides)
                {
                    var property = variant.getEditInterface().getEditableProperties().Where(i => i.getValue(0).Equals(o.Name)).FirstOrDefault();
                    if (property != null)
                    {
                        property.setValueStr(1, o.Value);
                    }
                }
            }

            private void buildEditInterface(String name)
            {
                ensureScannerExists();
                editInterface = new ReflectedListLikeEditInterface<VariantOverride>(overrides, name, () => new VariantOverride(this), validateCallback: () =>
                {
                    bool foundName = false;
                    bool nameValid = false;
                    foreach (var o in overrides)
                    {
                        if (!foundName)
                        {
                            foundName = o.Name == "Name";
                            nameValid = !String.IsNullOrWhiteSpace(o.Value);
                        }
                        if (!scanner.getMatchingMembers(typeof(MaterialDescription)).Where(i => i.getWrappedName() == o.Name).Any())
                        {
                            throw new ValidationException(String.Format("Cannot find a property named '{0}' to override. Please remove this property.", o.Name));
                        }
                    }
                    if (!foundName)
                    {
                        throw new ValidationException("You must have one override property named 'Name' with a unique material name.");
                    }
                    if (!nameValid)
                    {
                        throw new ValidationException("Name property must contain a value.");
                    }
                });
            }

            public EditInterface EditInterface
            {
                get
                {
                    return editInterface;
                }
            }
        }

        class VariantOverride
        {
            VariantProperties properties;

            private String value;

            public VariantOverride(VariantProperties properties)
            {
                this.properties = properties;
            }

            public VariantOverride(String name, String value, VariantProperties properties)
            {
                this.Name = name;
                this.value = value;
                this.properties = properties;
            }

            [Editable]
            public String Name { get; set; }

            [Editable]
            public String Value
            {
                get
                {
                    return value;
                }
                set
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        properties.changed(this);
                    }
                }
            }
        }
    }
}
