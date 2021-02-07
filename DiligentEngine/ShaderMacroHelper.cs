using System;
using System.Collections.Generic;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;
using System.Linq;

namespace DiligentEngine
{
    public class ShaderMacroHelper
    {
        private List<(String name, String definition)> macros = new List<(string, string)>();

        internal MacroPassStruct[] CreatePassStructArray()
        {
            return macros.Select(i => new MacroPassStruct
            {
                name = i.name,
                definition = i.definition
            }).ToArray();
        }

        public void RemoveMacro(String Name)
        {
            int index = GetIndex(Name);
            if (index > -1)
            {
                macros.RemoveAt(index);
            }
        }

        public void AddShaderMacro(String Name, String Definition)
        {
            int index = GetIndex(Name);
            if (index > -1)
            {
                macros[index] = (Name, Definition);
            }
            else
            {
                macros.Add((Name, Definition));
            }
        }

        public void AddShaderMacro(String Name, bool Definition)
        {
            AddShaderMacro(Name, Definition ? "1" : "0");
        }

        public void AddShaderMacro(String Name, float Definition)
        {
            AddShaderMacro(Name, Definition.ToString());
        }

        public void AddShaderMacro(String Name, Int32 Definition)
        {
            AddShaderMacro(Name, Definition.ToString());
        }

        public void AddShaderMacro(String Name, Uint32 Definition)
        {
            // Make sure that uint constants have the 'u' suffix to avoid problems in GLES.
            AddShaderMacro(Name, Definition.ToString() + "u");
        }

        public void AddShaderMacro(String Name, Uint8 Definition)
        {
            AddShaderMacro(Name, (Uint32)Definition);
        }

        private int GetIndex(string Name)
        {
            return macros.FindIndex(i => i.name == Name);
        }
    }
}
