using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;

namespace Engine.Reflection
{
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
        private static Dictionary<Type, Type> typeMapping = new Dictionary<Type, Type>();

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ReflectedVariable()
        {
            typeMapping.Add(typeof(String), typeof(StringReflectedVariable));
            typeMapping.Add(typeof(int), typeof(IntReflectedVariable));
            typeMapping.Add(typeof(long), typeof(LongReflectedVariable));
            typeMapping.Add(typeof(float), typeof(FloatReflectedVariable));
            typeMapping.Add(typeof(double), typeof(DoubleReflectedVariable));
            typeMapping.Add(typeof(Quaternion), typeof(QuaternionReflectedVariable));
            typeMapping.Add(typeof(Vector3), typeof(Vector3ReflectedVariable));
            typeMapping.Add(typeof(Vector2), typeof(Vector2ReflectedVariable));
            typeMapping.Add(typeof(bool), typeof(BooleanReflectedVariable));
            typeMapping.Add(typeof(short), typeof(ShortReflectedVariable));
            typeMapping.Add(typeof(ushort), typeof(UShortReflectedVariable));
            typeMapping.Add(typeof(uint), typeof(UIntReflectedVariable));
            typeMapping.Add(typeof(byte), typeof(ByteReflectedVariable));
            typeMapping.Add(typeof(Color), typeof(ColorReflectedVariable));
            typeMapping.Add(typeof(Decimal), typeof(DecimalReflectedVariable));
            typeMapping.Add(typeof(Size2), typeof(Size2ReflectedVariable));
            typeMapping.Add(typeof(IntSize2), typeof(IntSize2ReflectedVariable));
        }

        /// <summary>
        /// Determine if a mapping exists to create a ReflectedVariable for the
        /// given type.
        /// </summary>
        /// <param name="inType">The type to check.</param>
        /// <returns>True if a ReflectedVariable can be created for this type.</returns>
        public static bool canCreateVariable(Type inType)
        {
            return typeMapping.ContainsKey(inType) || inType.IsEnum;
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
                return (ReflectedVariable)Activator.CreateInstance(typeMapping[inType], memberInfo, instance);
            }
            else if (inType.IsEnum)
            {
                return (ReflectedVariable)Activator.CreateInstance(typeof(EnumReflectedVariable), memberInfo, instance);
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
