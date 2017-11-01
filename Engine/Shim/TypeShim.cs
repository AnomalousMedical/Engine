using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeShim
    {
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static Assembly Assembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsSubclassOf(this Type type, Type parent)
        {
            return type.GetTypeInfo().IsSubclassOf(parent);
        }

        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            if (interfaceType.IsGenericType())
            {
                interfaceType = interfaceType.GetGenericTypeDefinition();
            }
            return type.GetTypeInfo().ImplementedInterfaces.Any(i =>
            {
                Type checkType = i;
                if (i.IsGenericType())
                {
                    checkType = i.GetGenericTypeDefinition();
                }
                return interfaceType == checkType;
            });
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attrType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attrType, inherit).Cast<Attribute>();
        }

        public static FieldInfo[] GetFields(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetFields(flags);
        }

        public static Type GetInterface(this Type type, String name)
        {
            return type.GetTypeInfo().GetInterface(name);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetProperties(flags);
        }
    }
}