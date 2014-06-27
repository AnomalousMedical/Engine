using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class EnumUtil
    {
        public static IEnumerable<String> Elements(Type enumType)
        {
            foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                yield return fieldInfo.Name;
            }
        } 
    }
}
