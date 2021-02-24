using DiligentEngine;
using DiligentEngine.GltfPbr;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class SceneObject
    {
        public Vector3 position;
        public Quaternion orientation;
        public Vector3 scale;
        public IBuffer vertexBuffer;
        public IBuffer skinVertexBuffer;
        public IBuffer indexBuffer;
        public uint numIndices;
        public IShaderResourceBinding shaderResourceBinding;
        public PbrAlphaMode pbrAlphaMode;
        public bool GetShadows;
        public bool RenderShadow;
        public Sprite Sprite;
    }
}
