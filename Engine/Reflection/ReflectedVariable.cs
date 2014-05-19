using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;

namespace Engine.Reflection
{
    public delegate ReflectedVariable ReflectedTypeCreatorDelegate(MemberWrapper memberInfo, Object instance);

    /// <summary>
    /// This abstract class is a supertype for types that can be set or
    /// retrieved directly.
    /// </summary>
    public abstract class ReflectedVariable
    {
        #region TypeMapping

        /// <summary>
        /// This holds a mapping of basic types to the matching instance variable.
        /// </summary>
        private static Dictionary<Type, ReflectedTypeCreatorDelegate> typeMapping = new Dictionary<Type, ReflectedTypeCreatorDelegate>();

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ReflectedVariable()
        {
            typeMapping.Add(typeof(String), (memberInfo, instance) => new StringReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(int), (memberInfo, instance) => new IntReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(int?), (memberInfo, instance) => new IntNullableReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(long), (memberInfo, instance) => new LongReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(float), (memberInfo, instance) => new FloatReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(double), (memberInfo, instance) => new DoubleReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Quaternion), (memberInfo, instance) => new QuaternionReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Vector3), (memberInfo, instance) => new Vector3ReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Vector2), (memberInfo, instance) => new Vector2ReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(bool), (memberInfo, instance) => new BooleanReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(short), (memberInfo, instance) => new ShortReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(ushort), (memberInfo, instance) => new UShortReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(uint), (memberInfo, instance) => new UIntReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(byte), (memberInfo, instance) => new ByteReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Color), (memberInfo, instance) => new ColorReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Color?), (memberInfo, instance) => new ColorNullableReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Decimal), (memberInfo, instance) => new DecimalReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(Size2), (memberInfo, instance) => new Size2ReflectedVariable(memberInfo, instance));
            typeMapping.Add(typeof(IntSize2), (memberInfo, instance) => new IntSize2ReflectedVariable(memberInfo, instance));
        }

        /// <summary>
        /// Determine if a mapping exists to create a ReflectedVariable for the
        /// given type.
        /// </summary>
        /// <param name="inType">The type to check.</param>
        /// <returns>True if a ReflectedVariable can be created for this type.</returns>
        public static bool canCreateVariable(Type inType)
        {
            return typeMapping.ContainsKey(inType) || inType.IsEnum();
        }

        /// <summary>
        /// Returns the ReflectedVariable for the given type.
        /// </summary>
        /// <param name="memberInfo">The MemberWrapper to use in the ReflectedVariable.</param>
        /// <param name="instance">The instance to use in the ReflectedVariable.</param>
        /// <returns>The matching ReflectedVariable or null if no mapping exists.</returns>
        public static ReflectedVariable createVariable(MemberWrapper memberInfo, Object instance)
        {
            Type inType = memberInfo.getWrappedType();
            if (typeMapping.ContainsKey(inType))
            {
                return typeMapping[inType](memberInfo, instance);
            }
            else if (inType.IsEnum())
            {
                return new EnumReflectedVariable(memberInfo, instance);
            }
            else
            {
                return null;
            }
        }

        #endregion TypeMapping

        #region Fields

        protected MemberWrapper propertyInfo;
        private object instance;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        /// <param name="visibility">The visibility of the variable.</param>
        public ReflectedVariable(MemberWrapper propInfo, Object instance)
        {
            this.propertyInfo = propInfo;
            this.instance = instance;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Determine if the given object is a valid value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True if value can be set as the value of this variable.</returns>
        public virtual bool canSetValue(Object value)
        {
            return value.GetType() == propertyInfo.getWrappedType();
        }

        /// <summary>
        /// Get the value as an object.
        /// </summary>
        /// <returns>The value as an object.</returns>
        public object getValue()
        {
            return propertyInfo.getValue(instance, null);
        }

        /// <summary>
        /// Set the value as an object.
        /// </summary>
        /// <param name="value">An object with the value, must be of the correct type.</param>
        public void setValue(object value)
        {
            propertyInfo.setValue(instance, value, null);
        }

        /// <summary>
        /// Determines if value can be parsed to the underlying type.
        /// </summary>
        /// <returns>True if the String can be parsed correctly.</returns>
        public abstract bool canParseString(String value);

        /// <summary>
        /// Get the value of this type as a string.
        /// </summary>
        /// <returns>The value of this type as a string.</returns>
        public virtual String getValueString()
        {
            return getValue().ToString();
        }

        /// <summary>
        /// Set the value of this type as a string.
        /// </summary>
        /// <param name="value">The string to set as the value.</param>
        public abstract void setValueString(String value);

        /// <summary>
        /// Get the type of this variable.
        /// </summary>
        /// <returns>The type of this variable.</returns>
        public Type getVariableType()
        {
            return propertyInfo.getWrappedType();
        }

        public Browser ItemBrowser { get; set; }

        public bool HasItemBrowser { get; set; }

        #endregion Functions
    }
}
